using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RotmgManager.Models;

namespace RotmgManager.UI;

public class AddCharacterForm : Form
{
    private readonly TextBox nameTextBox;
    private readonly ComboBox classComboBox;

    public string CharacterName => nameTextBox.Text.Trim();
    public string SelectedClassName => classComboBox.SelectedItem as string ?? string.Empty;

    public AddCharacterForm(IEnumerable<ClassConfig> classConfigs)
    {
        Text = "Add Character";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(10);

        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 3,
            Dock = DockStyle.Fill,
            AutoSize = true
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var nameLabel = new Label
        {
            Text = "Name:",
            Anchor = AnchorStyles.Left,
            AutoSize = true
        };

        nameTextBox = new TextBox
        {
            Dock = DockStyle.Fill
        };

        var classLabel = new Label
        {
            Text = "Class:",
            Anchor = AnchorStyles.Left,
            AutoSize = true
        };

        classComboBox = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        classComboBox.Items.AddRange(classConfigs.Select(c => c.ClassName).ToArray());
        if (classComboBox.Items.Count > 0)
        {
            classComboBox.SelectedIndex = 0;
        }

        var buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill,
            AutoSize = true
        };

        var okButton = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            AutoSize = true
        };
        okButton.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(CharacterName) || string.IsNullOrWhiteSpace(SelectedClassName))
            {
                MessageBox.Show(this, "Enter a name and choose a class.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        };

        var cancelButton = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            AutoSize = true
        };

        buttonPanel.Controls.Add(okButton);
        buttonPanel.Controls.Add(cancelButton);

        layout.Controls.Add(nameLabel, 0, 0);
        layout.Controls.Add(nameTextBox, 1, 0);
        layout.Controls.Add(classLabel, 0, 1);
        layout.Controls.Add(classComboBox, 1, 1);
        layout.Controls.Add(buttonPanel, 0, 2);
        layout.SetColumnSpan(buttonPanel, 2);

        Controls.Add(layout);

        AcceptButton = okButton;
        CancelButton = cancelButton;
    }
}
