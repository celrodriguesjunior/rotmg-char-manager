using System.Windows.Forms;

namespace RotmgManager;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null!;
    private TabControl mainTabControl = null!;
    private TabPage manageTabPage = null!;
    private TabPage overviewTabPage = null!;
    private TableLayoutPanel mainTableLayoutPanel = null!;
    private GroupBox vaultGroupBox = null!;
    private TableLayoutPanel vaultTableLayoutPanel = null!;
    private TableLayoutPanel centerTableLayoutPanel = null!;
    private ComboBox characterComboBox = null!;
    private Button addCharacterButton = null!;
    private Button deleteCharacterButton = null!;
    private Label nameLabel = null!;
    private TextBox characterNameTextBox = null!;
    private Label classLabel = null!;
    private ComboBox classComboBox = null!;
    private Label progressLabel = null!;
    private GroupBox statsGroupBox = null!;
    private TableLayoutPanel statsTableLayoutPanel = null!;
    private PictureBox classPictureBox = null!;
    private TableLayoutPanel overviewTableLayoutPanel = null!;
    private DataGridView overviewDataGridView = null!;
    private Label overviewTotalLabel = null!;
    private FlowLayoutPanel bottomFlowLayoutPanel = null!;
    private Button saveButton = null!;
    private Button recalcButton = null!;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            classIconProvider.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        mainTabControl = new TabControl();
        manageTabPage = new TabPage();
        mainTableLayoutPanel = new TableLayoutPanel();
        vaultGroupBox = new GroupBox();
        vaultTableLayoutPanel = new TableLayoutPanel();
        centerTableLayoutPanel = new TableLayoutPanel();
        characterComboBox = new ComboBox();
        addCharacterButton = new Button();
        deleteCharacterButton = new Button();
        nameLabel = new Label();
        characterNameTextBox = new TextBox();
        classLabel = new Label();
        classComboBox = new ComboBox();
        classPictureBox = new PictureBox();
        progressLabel = new Label();
        statsGroupBox = new GroupBox();
        statsTableLayoutPanel = new TableLayoutPanel();
        overviewTabPage = new TabPage();
        overviewTableLayoutPanel = new TableLayoutPanel();
        overviewDataGridView = new DataGridView();
        overviewTotalLabel = new Label();
        bottomFlowLayoutPanel = new FlowLayoutPanel();
        saveButton = new Button();
        recalcButton = new Button();
        SuspendLayout();
        // 
        // mainTabControl
        // 
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Controls.Add(manageTabPage);
        mainTabControl.Controls.Add(overviewTabPage);
        // 
        // manageTabPage
        // 
        manageTabPage.Text = "Manager";
        manageTabPage.Padding = new Padding(3);
        manageTabPage.Controls.Add(mainTableLayoutPanel);
        manageTabPage.UseVisualStyleBackColor = true;
        // 
        // mainTableLayoutPanel
        // 
        mainTableLayoutPanel.ColumnCount = 3;
        mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        mainTableLayoutPanel.Dock = DockStyle.Fill;
        mainTableLayoutPanel.RowCount = 1;
        mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainTableLayoutPanel.Controls.Add(vaultGroupBox, 0, 0);
        mainTableLayoutPanel.Controls.Add(centerTableLayoutPanel, 1, 0);
        mainTableLayoutPanel.Controls.Add(statsGroupBox, 2, 0);
        // 
        // vaultGroupBox
        // 
        vaultGroupBox.Text = "Vault Potions";
        vaultGroupBox.Dock = DockStyle.Fill;
        vaultGroupBox.Padding = new Padding(8);
        vaultGroupBox.Controls.Add(vaultTableLayoutPanel);
        // 
        // vaultTableLayoutPanel
        // 
        vaultTableLayoutPanel.ColumnCount = 2;
        vaultTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        vaultTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        vaultTableLayoutPanel.Dock = DockStyle.Fill;
        vaultTableLayoutPanel.AutoSize = true;
        // 
        // centerTableLayoutPanel
        // 
        centerTableLayoutPanel.ColumnCount = 2;
        centerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        centerTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        centerTableLayoutPanel.Dock = DockStyle.Fill;
        centerTableLayoutPanel.RowCount = 6;
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        centerTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        centerTableLayoutPanel.Padding = new Padding(8);
        centerTableLayoutPanel.Controls.Add(characterComboBox, 0, 0);
        centerTableLayoutPanel.SetColumnSpan(characterComboBox, 2);
        centerTableLayoutPanel.Controls.Add(addCharacterButton, 0, 1);
        centerTableLayoutPanel.Controls.Add(deleteCharacterButton, 1, 1);
        centerTableLayoutPanel.Controls.Add(nameLabel, 0, 2);
        centerTableLayoutPanel.Controls.Add(characterNameTextBox, 1, 2);
        centerTableLayoutPanel.Controls.Add(classLabel, 0, 3);
        centerTableLayoutPanel.Controls.Add(classComboBox, 1, 3);
        centerTableLayoutPanel.Controls.Add(classPictureBox, 0, 4);
        centerTableLayoutPanel.SetColumnSpan(classPictureBox, 2);
        centerTableLayoutPanel.Controls.Add(progressLabel, 0, 5);
        centerTableLayoutPanel.SetColumnSpan(progressLabel, 2);
        // 
        // characterComboBox
        // 
        characterComboBox.Dock = DockStyle.Fill;
        characterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        // 
        // addCharacterButton
        // 
        addCharacterButton.Text = "Add Character";
        addCharacterButton.AutoSize = true;
        // 
        // deleteCharacterButton
        // 
        deleteCharacterButton.Text = "Delete Character";
        deleteCharacterButton.AutoSize = true;
        deleteCharacterButton.Anchor = AnchorStyles.Right;
        // 
        // nameLabel
        // 
        nameLabel.Text = "Name:";
        nameLabel.Anchor = AnchorStyles.Left;
        nameLabel.AutoSize = true;
        // 
        // characterNameTextBox
        // 
        characterNameTextBox.Dock = DockStyle.Fill;
        // 
        // classLabel
        // 
        classLabel.Text = "Class:";
        classLabel.Anchor = AnchorStyles.Left;
        classLabel.AutoSize = true;
        // 
        // classComboBox
        // 
        classComboBox.Dock = DockStyle.Fill;
        classComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        // 
        // classPictureBox
        // 
        classPictureBox.Dock = DockStyle.Fill;
        classPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        classPictureBox.BorderStyle = BorderStyle.FixedSingle;
        classPictureBox.Margin = new Padding(0, 8, 0, 8);
        // 
        // progressLabel
        // 
        progressLabel.Text = "Stats maxed: 0 / 8";
        progressLabel.Dock = DockStyle.Fill;
        progressLabel.AutoSize = true;
        progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // statsGroupBox
        // 
        statsGroupBox.Text = "Character Stats";
        statsGroupBox.Dock = DockStyle.Fill;
        statsGroupBox.Padding = new Padding(8);
        statsGroupBox.Controls.Add(statsTableLayoutPanel);
        // 
        // statsTableLayoutPanel
        // 
        statsTableLayoutPanel.ColumnCount = 4;
        statsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        statsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        statsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        statsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        statsTableLayoutPanel.Dock = DockStyle.Fill;
        statsTableLayoutPanel.AutoSize = true;
        // 
        // overviewTabPage
        // 
        overviewTabPage.Text = "Overview";
        overviewTabPage.Padding = new Padding(3);
        overviewTabPage.UseVisualStyleBackColor = true;
        overviewTabPage.Controls.Add(overviewTableLayoutPanel);
        // 
        // overviewTableLayoutPanel
        // 
        overviewTableLayoutPanel.ColumnCount = 1;
        overviewTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        overviewTableLayoutPanel.Dock = DockStyle.Fill;
        overviewTableLayoutPanel.RowCount = 2;
        overviewTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        overviewTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        overviewTableLayoutPanel.Padding = new Padding(8);
        overviewTableLayoutPanel.Controls.Add(overviewDataGridView, 0, 0);
        overviewTableLayoutPanel.Controls.Add(overviewTotalLabel, 0, 1);
        // 
        // overviewDataGridView
        // 
        overviewDataGridView.AllowUserToAddRows = false;
        overviewDataGridView.AllowUserToDeleteRows = false;
        overviewDataGridView.ReadOnly = true;
        overviewDataGridView.RowHeadersVisible = false;
        overviewDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        overviewDataGridView.Dock = DockStyle.Fill;
        overviewDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        // 
        // overviewTotalLabel
        // 
        overviewTotalLabel.Text = "Total pots needed: 0";
        overviewTotalLabel.Dock = DockStyle.Fill;
        overviewTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        overviewTotalLabel.AutoSize = true;
        // 
        // bottomFlowLayoutPanel
        // 
        bottomFlowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
        bottomFlowLayoutPanel.Dock = DockStyle.Bottom;
        bottomFlowLayoutPanel.AutoSize = true;
        bottomFlowLayoutPanel.Padding = new Padding(8);
        bottomFlowLayoutPanel.Controls.Add(saveButton);
        bottomFlowLayoutPanel.Controls.Add(recalcButton);
        // 
        // saveButton
        // 
        saveButton.Text = "Save";
        saveButton.AutoSize = true;
        // 
        // recalcButton
        // 
        recalcButton.Text = "Recalculate";
        recalcButton.AutoSize = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1000, 600);
        Controls.Add(mainTabControl);
        Controls.Add(bottomFlowLayoutPanel);
        MinimumSize = new System.Drawing.Size(900, 500);
        Text = "RotMG Stat Manager";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
