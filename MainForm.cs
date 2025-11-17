using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using RotmgManager.Models;
using RotmgManager.Persistence;
using RotmgManager.Services;
using RotmgManager.UI;

namespace RotmgManager;

public partial class MainForm : Form
{
    private readonly GameDataRepository repository;
    private readonly GameData gameData;
    private readonly CharacterService characterService;
    private readonly ClassIconProvider classIconProvider = new();
    private readonly BindingList<Character> characterBinding;
    private readonly Dictionary<StatType, NumericUpDown> vaultInputs = new();
    private readonly Dictionary<StatType, NumericUpDown> statInputs = new();
    private readonly Dictionary<StatType, Label> maxLabels = new();
    private readonly Dictionary<StatType, Label> missingLabels = new();
    private bool initializing;

    public MainForm(GameDataRepository repository, GameData data, CharacterService service)
    {
        InitializeComponent();
        this.repository = repository;
        gameData = data;
        characterService = service;

        classComboBox.DataSource = gameData.Classes;
        classComboBox.DisplayMember = nameof(ClassConfig.ClassName);
        classComboBox.ValueMember = nameof(ClassConfig.ClassName);

        characterBinding = new BindingList<Character>(gameData.Characters);
        characterComboBox.DataSource = characterBinding;
        characterComboBox.DisplayMember = nameof(Character.Name);
        characterComboBox.ValueMember = nameof(Character.Id);

        HookEvents();
        InitializeVaultPanel();
        InitializeStatsPanel();
        InitializeOverviewTab();
        LoadVaultValues();

        if (characterBinding.Count > 0)
        {
            characterComboBox.SelectedIndex = 0;
        }
        else
        {
            ClearCharacterDetails();
        }

        RefreshOverview();
    }

    private void HookEvents()
    {
        characterComboBox.SelectedIndexChanged += (_, _) => LoadSelectedCharacter();
        addCharacterButton.Click += (_, _) => AddCharacter();
        deleteCharacterButton.Click += (_, _) => DeleteSelectedCharacter();
        characterNameTextBox.TextChanged += (_, _) =>
        {
            if (!initializing)
            {
                UpdateCharacterName();
            }
        };
        classComboBox.SelectedIndexChanged += (_, _) =>
        {
            if (!initializing)
            {
                UpdateCharacterClass();
            }
        };
        saveButton.Click += (_, _) => SaveGameData();
        recalcButton.Click += (_, _) =>
        {
            var character = GetCurrentCharacter();
            if (character != null)
            {
                RefreshMissingPotLabels(character);
            }
        };
    }

    private void InitializeVaultPanel()
    {
        vaultTableLayoutPanel.SuspendLayout();
        vaultTableLayoutPanel.Controls.Clear();
        vaultTableLayoutPanel.RowStyles.Clear();

        int row = 0;
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            vaultTableLayoutPanel.RowCount = row + 1;
            vaultTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var label = new Label
            {
                Text = stat.ToString(),
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = true
            };

            var numeric = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 999,
                Dock = DockStyle.Fill,
                Tag = stat
            };
            numeric.ValueChanged += (_, _) =>
            {
                if (!initializing)
                {
                    UpdateVaultPotion(stat, (int)numeric.Value);
                }
            };

            vaultInputs[stat] = numeric;
            vaultTableLayoutPanel.Controls.Add(label, 0, row);
            vaultTableLayoutPanel.Controls.Add(numeric, 1, row);
            row++;
        }

        vaultTableLayoutPanel.ResumeLayout();
    }

    private void InitializeStatsPanel()
    {
        statsTableLayoutPanel.SuspendLayout();
        statsTableLayoutPanel.Controls.Clear();
        statsTableLayoutPanel.RowStyles.Clear();

        int row = 0;
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            statsTableLayoutPanel.RowCount = row + 1;
            statsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var statLabel = new Label
            {
                Text = stat.ToString(),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            var currentNumeric = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 999,
                Dock = DockStyle.Fill,
                Tag = stat
            };
            currentNumeric.ValueChanged += (_, _) =>
            {
                if (!initializing)
                {
                    UpdateCharacterStat(stat, (int)currentNumeric.Value);
                }
            };

            var maxLabel = new Label
            {
                Text = "-",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                AutoSize = true
            };

            var missingLabel = new Label
            {
                Text = "-",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                AutoSize = true
            };

            statInputs[stat] = currentNumeric;
            maxLabels[stat] = maxLabel;
            missingLabels[stat] = missingLabel;

            statsTableLayoutPanel.Controls.Add(statLabel, 0, row);
            statsTableLayoutPanel.Controls.Add(currentNumeric, 1, row);
            statsTableLayoutPanel.Controls.Add(maxLabel, 2, row);
            statsTableLayoutPanel.Controls.Add(missingLabel, 3, row);
            row++;
        }

        statsTableLayoutPanel.ResumeLayout();
    }

    private void InitializeOverviewTab()
    {
        overviewDataGridView.Columns.Clear();
        overviewDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        overviewDataGridView.AutoGenerateColumns = false;
        overviewDataGridView.RowTemplate.Height = 64;

        overviewDataGridView.Columns.Add(new DataGridViewImageColumn
        {
            Name = "ClassIconColumn",
            HeaderText = "",
            ImageLayout = DataGridViewImageCellLayout.Zoom,
            Width = 60,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        overviewDataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "CharacterColumn",
            HeaderText = "Character",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        });

        overviewDataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ClassColumn",
            HeaderText = "Class",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        });

        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            overviewDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = $"{stat}Column",
                HeaderText = stat.ToString(),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        overviewDataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "TotalMissingColumn",
            HeaderText = "Total Pots",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        });
    }

    private void LoadVaultValues()
    {
        initializing = true;
        foreach ((StatType stat, NumericUpDown control) in vaultInputs)
        {
            gameData.Vault.Potions.TryGetValue(stat, out int value);
            control.Value = Math.Max(control.Minimum, Math.Min(control.Maximum, value));
        }

        initializing = false;
    }

    private void LoadSelectedCharacter()
    {
        var character = GetCurrentCharacter();
        if (character == null)
        {
            ClearCharacterDetails();
            return;
        }

        SetCharacterControlsEnabled(true);
        initializing = true;
        characterNameTextBox.Text = character.Name;

        var classMatch = gameData.Classes.FirstOrDefault(c =>
            string.Equals(c.ClassName, character.ClassName, StringComparison.OrdinalIgnoreCase));
        if (classMatch != null)
        {
            classComboBox.SelectedItem = classMatch;
        }
        else if (gameData.Classes.Count > 0)
        {
            classComboBox.SelectedIndex = 0;
            if (classComboBox.SelectedItem is ClassConfig defaultClass)
            {
                character.ClassName = defaultClass.ClassName;
            }
        }
        initializing = false;

        LoadStatsForCharacter(character);
        UpdateClassImage(character.ClassName);
    }

    private void LoadStatsForCharacter(Character character)
    {
        StatDictionaryFactory.EnsureAllStats(character.CurrentStats);
        UpdateClassImage(character.ClassName);
        initializing = true;
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            statInputs[stat].Value = ClampToNumericRange(statInputs[stat], character.CurrentStats[stat]);
        }

        initializing = false;
        RefreshMissingPotLabels(character);
    }

    private void ClearCharacterDetails()
    {
        SetCharacterControlsEnabled(false);
        initializing = true;
        characterNameTextBox.Text = string.Empty;
        classComboBox.SelectedIndex = -1;
        foreach (NumericUpDown numeric in statInputs.Values)
        {
            numeric.Value = numeric.Minimum;
        }

        initializing = false;
        progressLabel.Text = "Stats maxed: 0 / 8";

        foreach (Label label in maxLabels.Values)
        {
            label.Text = "-";
        }
        foreach (Label label in missingLabels.Values)
        {
            label.Text = "-";
        }

        UpdateClassImage(null);
    }

    private void SetCharacterControlsEnabled(bool enabled)
    {
        characterNameTextBox.Enabled = enabled;
        classComboBox.Enabled = enabled;
        deleteCharacterButton.Enabled = enabled;
        recalcButton.Enabled = enabled;
        foreach (NumericUpDown numeric in statInputs.Values)
        {
            numeric.Enabled = enabled;
        }
    }

    private Character? GetCurrentCharacter()
    {
        return characterComboBox.SelectedItem as Character;
    }

    private void UpdateCharacterName()
    {
        var character = GetCurrentCharacter();
        if (character == null)
        {
            return;
        }

        character.Name = characterNameTextBox.Text;
        characterBinding.ResetBindings();
        RefreshOverview();
    }

    private void UpdateCharacterClass()
    {
        var character = GetCurrentCharacter();
        if (character == null || classComboBox.SelectedItem is not ClassConfig selectedClass)
        {
            return;
        }

        character.ClassName = selectedClass.ClassName;
        LoadStatsForCharacter(character);
        UpdateClassImage(character.ClassName);
    }

    private void UpdateCharacterStat(StatType stat, int value)
    {
        var character = GetCurrentCharacter();
        if (character == null)
        {
            return;
        }

        character.CurrentStats[stat] = value;
        RefreshMissingPotLabels(character);
    }

    private void UpdateVaultPotion(StatType stat, int value)
    {
        gameData.Vault.Potions[stat] = value;
    }

    private void RefreshMissingPotLabels(Character character)
    {
        var config = characterService.GetClassConfig(character);
        var missing = characterService.GetMissingPots(character);
        int maxedCount = 0;
        int totalStats = Enum.GetValues(typeof(StatType)).Length;

        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            int maxValue = 0;
            if (config != null && config.MaxStats.TryGetValue(stat, out int configuredMax))
            {
                maxValue = configuredMax;
            }

            maxLabels[stat].Text = maxValue > 0 ? maxValue.ToString() : "-";
            missingLabels[stat].Text = missing[stat].ToString();

            character.CurrentStats.TryGetValue(stat, out int current);
            if (maxValue > 0 && current >= maxValue)
            {
                maxedCount++;
            }
        }

        progressLabel.Text = $"Stats maxed: {maxedCount} / {totalStats}";
        RefreshOverview();
    }

    private void AddCharacter()
    {
        using var dialog = new AddCharacterForm(gameData.Classes);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var character = new Character
        {
            Name = dialog.CharacterName,
            ClassName = dialog.SelectedClassName,
            CurrentStats = StatDictionaryFactory.Create(0)
        };

        characterBinding.Add(character);
        characterComboBox.SelectedItem = character;
        RefreshOverview();
    }

    private void DeleteSelectedCharacter()
    {
        var character = GetCurrentCharacter();
        if (character == null)
        {
            return;
        }

        if (MessageBox.Show(this, $"Delete {character.Name}?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        int index = characterComboBox.SelectedIndex;
        characterBinding.Remove(character);

        if (characterBinding.Count == 0)
        {
            ClearCharacterDetails();
        }
        else
        {
            characterComboBox.SelectedIndex = Math.Max(0, index - 1);
        }

        RefreshOverview();
    }

    private void SaveGameData()
    {
        repository.Save(gameData);
        MessageBox.Show(this, "Game data saved.", "Save",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static decimal ClampToNumericRange(NumericUpDown control, int value)
    {
        if (value < control.Minimum)
        {
            return control.Minimum;
        }

        if (value > control.Maximum)
        {
            return control.Maximum;
        }

        return value;
    }

    private void RefreshOverview()
    {
        overviewDataGridView.Rows.Clear();
        int totalMissing = 0;
        var totalMissingByStat = StatDictionaryFactory.Create(0);

        foreach (Character character in gameData.Characters)
        {
            var missing = characterService.GetMissingPots(character);
            var config = characterService.GetClassConfig(character);
            int characterMissingTotal = missing.Values.Sum();
            totalMissing += characterMissingTotal;

            var rowValues = new List<object>
            {
                classIconProvider.GetIcon(character.ClassName),
                character.Name,
                character.ClassName
            };

            foreach (StatType stat in Enum.GetValues(typeof(StatType)))
            {
                int statMissing = missing[stat];
                totalMissingByStat[stat] += statMissing;
                rowValues.Add(FormatStatSummary(character, config, stat, statMissing));
            }

            rowValues.Add(characterMissingTotal);
            overviewDataGridView.Rows.Add(rowValues.ToArray());
        }

        var totalParts = new List<string> { $"Total pots needed: {totalMissing}" };
        totalParts.AddRange(Enum.GetValues(typeof(StatType))
            .Cast<StatType>()
            .Select(stat => $"{stat}: {totalMissingByStat[stat]}"));

        overviewTotalLabel.Text = string.Join(" | ", totalParts);
    }

    private static string FormatStatSummary(Character character, ClassConfig? config, StatType stat, int missing)
    {
        character.CurrentStats.TryGetValue(stat, out int current);
        int maxValue = 0;
        if (config != null && config.MaxStats.TryGetValue(stat, out int configuredMax))
        {
            maxValue = configuredMax;
        }

        string maxText = maxValue > 0 ? maxValue.ToString() : "-";
        return $"{current}/{maxText} ({missing})";
    }

    private void UpdateClassImage(string? className)
    {
        classPictureBox.Image = classIconProvider.GetIcon(className ?? string.Empty);
    }
}
