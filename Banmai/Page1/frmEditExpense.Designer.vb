﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditExpense
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Guna2Elipse1 = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.btnPrint = New Guna.UI2.WinForms.Guna2Button()
        Me.btnDelete = New Guna.UI2.WinForms.Guna2Button()
        Me.btnSave = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2GroupBox2 = New Guna.UI2.WinForms.Guna2GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtDetailAmount = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDetailName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDetailID = New Guna.UI2.WinForms.Guna2TextBox()
        Me.dgvExpenses = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2GroupBox1 = New Guna.UI2.WinForms.Guna2GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbAccount = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.dtpExpenseDate = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Me.txtExpenseID = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtExpenseDescription = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtExpenseAmount = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtExpenseName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.dgvExpenseDetails = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Guna2HtmlLabel11 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Guna2GroupBox2.SuspendLayout()
        CType(Me.dgvExpenses, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2GroupBox1.SuspendLayout()
        CType(Me.dgvExpenseDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Elipse1
        '
        Me.Guna2Elipse1.TargetControl = Me
        '
        'btnPrint
        '
        Me.btnPrint.Animated = True
        Me.btnPrint.BorderRadius = 10
        Me.btnPrint.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPrint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPrint.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPrint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPrint.FillColor = System.Drawing.Color.Gold
        Me.btnPrint.Font = New System.Drawing.Font("FC Minimal", 15.75!, System.Drawing.FontStyle.Bold)
        Me.btnPrint.ForeColor = System.Drawing.Color.White
        Me.btnPrint.Location = New System.Drawing.Point(835, 895)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(215, 73)
        Me.btnPrint.TabIndex = 39
        Me.btnPrint.Text = "พิมพ์เอกสาร"
        '
        'btnDelete
        '
        Me.btnDelete.Animated = True
        Me.btnDelete.BorderRadius = 10
        Me.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnDelete.FillColor = System.Drawing.Color.Red
        Me.btnDelete.Font = New System.Drawing.Font("FC Minimal", 15.75!, System.Drawing.FontStyle.Bold)
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(614, 895)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(215, 73)
        Me.btnDelete.TabIndex = 37
        Me.btnDelete.Text = "ลบ"
        '
        'btnSave
        '
        Me.btnSave.Animated = True
        Me.btnSave.BorderRadius = 10
        Me.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnSave.FillColor = System.Drawing.Color.ForestGreen
        Me.btnSave.Font = New System.Drawing.Font("FC Minimal", 15.75!, System.Drawing.FontStyle.Bold)
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(393, 895)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(215, 73)
        Me.btnSave.TabIndex = 38
        Me.btnSave.Text = "บันทึก"
        '
        'Guna2GroupBox2
        '
        Me.Guna2GroupBox2.Controls.Add(Me.Label4)
        Me.Guna2GroupBox2.Controls.Add(Me.Label13)
        Me.Guna2GroupBox2.Controls.Add(Me.Label9)
        Me.Guna2GroupBox2.Controls.Add(Me.Label10)
        Me.Guna2GroupBox2.Controls.Add(Me.Label8)
        Me.Guna2GroupBox2.Controls.Add(Me.txtDetailAmount)
        Me.Guna2GroupBox2.Controls.Add(Me.txtDetailName)
        Me.Guna2GroupBox2.Controls.Add(Me.txtDetailID)
        Me.Guna2GroupBox2.CustomBorderColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2GroupBox2.Font = New System.Drawing.Font("FC Minimal", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2GroupBox2.ForeColor = System.Drawing.Color.White
        Me.Guna2GroupBox2.Location = New System.Drawing.Point(12, 688)
        Me.Guna2GroupBox2.Name = "Guna2GroupBox2"
        Me.Guna2GroupBox2.Size = New System.Drawing.Size(1876, 201)
        Me.Guna2GroupBox2.TabIndex = 35
        Me.Guna2GroupBox2.Text = "รายละเอียดรายจ่าย"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.White
        Me.Label4.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(882, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 32)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "บาท"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.White
        Me.Label13.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(986, 78)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(53, 32)
        Me.Label13.TabIndex = 24
        Me.Label13.Text = "บาท"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Window
        Me.Label9.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(529, 70)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(131, 32)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "จำนวนเงิน :"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.White
        Me.Label10.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(138, 153)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(106, 32)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "รายการ :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.White
        Me.Label8.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(52, 70)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(192, 32)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "เลขที่รายละเอียด :"
        '
        'txtDetailAmount
        '
        Me.txtDetailAmount.Animated = True
        Me.txtDetailAmount.BackColor = System.Drawing.Color.White
        Me.txtDetailAmount.BorderRadius = 10
        Me.txtDetailAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDetailAmount.DefaultText = ""
        Me.txtDetailAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDetailAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDetailAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailAmount.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetailAmount.ForeColor = System.Drawing.Color.Black
        Me.txtDetailAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailAmount.Location = New System.Drawing.Point(667, 62)
        Me.txtDetailAmount.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDetailAmount.Name = "txtDetailAmount"
        Me.txtDetailAmount.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtDetailAmount.PlaceholderText = ""
        Me.txtDetailAmount.SelectedText = ""
        Me.txtDetailAmount.Size = New System.Drawing.Size(208, 48)
        Me.txtDetailAmount.TabIndex = 1
        Me.txtDetailAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDetailName
        '
        Me.txtDetailName.Animated = True
        Me.txtDetailName.BackColor = System.Drawing.Color.White
        Me.txtDetailName.BorderRadius = 10
        Me.txtDetailName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDetailName.DefaultText = ""
        Me.txtDetailName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDetailName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDetailName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailName.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetailName.ForeColor = System.Drawing.Color.Black
        Me.txtDetailName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailName.Location = New System.Drawing.Point(251, 145)
        Me.txtDetailName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDetailName.Name = "txtDetailName"
        Me.txtDetailName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtDetailName.PlaceholderText = ""
        Me.txtDetailName.SelectedText = ""
        Me.txtDetailName.Size = New System.Drawing.Size(208, 48)
        Me.txtDetailName.TabIndex = 1
        '
        'txtDetailID
        '
        Me.txtDetailID.BackColor = System.Drawing.Color.White
        Me.txtDetailID.BorderRadius = 10
        Me.txtDetailID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDetailID.DefaultText = ""
        Me.txtDetailID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDetailID.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDetailID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetailID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailID.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetailID.ForeColor = System.Drawing.Color.Black
        Me.txtDetailID.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetailID.Location = New System.Drawing.Point(251, 62)
        Me.txtDetailID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDetailID.Name = "txtDetailID"
        Me.txtDetailID.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtDetailID.PlaceholderText = ""
        Me.txtDetailID.ReadOnly = True
        Me.txtDetailID.SelectedText = ""
        Me.txtDetailID.Size = New System.Drawing.Size(208, 48)
        Me.txtDetailID.TabIndex = 0
        '
        'dgvExpenses
        '
        Me.dgvExpenses.AllowUserToAddRows = False
        Me.dgvExpenses.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.dgvExpenses.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvExpenses.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvExpenses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvExpenses.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvExpenses.ColumnHeadersHeight = 70
        Me.dgvExpenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvExpenses.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvExpenses.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenses.Location = New System.Drawing.Point(12, 113)
        Me.dgvExpenses.Name = "dgvExpenses"
        Me.dgvExpenses.ReadOnly = True
        Me.dgvExpenses.RowHeadersVisible = False
        Me.dgvExpenses.Size = New System.Drawing.Size(920, 342)
        Me.dgvExpenses.TabIndex = 33
        Me.dgvExpenses.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenses.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.dgvExpenses.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.dgvExpenses.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.dgvExpenses.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.dgvExpenses.ThemeStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenses.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenses.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenses.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgvExpenses.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dgvExpenses.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.dgvExpenses.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        Me.dgvExpenses.ThemeStyle.HeaderStyle.Height = 70
        Me.dgvExpenses.ThemeStyle.ReadOnly = True
        Me.dgvExpenses.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenses.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvExpenses.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dgvExpenses.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.dgvExpenses.ThemeStyle.RowsStyle.Height = 22
        Me.dgvExpenses.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenses.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'Guna2GroupBox1
        '
        Me.Guna2GroupBox1.BackColor = System.Drawing.Color.White
        Me.Guna2GroupBox1.Controls.Add(Me.Label12)
        Me.Guna2GroupBox1.Controls.Add(Me.Label3)
        Me.Guna2GroupBox1.Controls.Add(Me.Label7)
        Me.Guna2GroupBox1.Controls.Add(Me.Label6)
        Me.Guna2GroupBox1.Controls.Add(Me.Label1)
        Me.Guna2GroupBox1.Controls.Add(Me.Label5)
        Me.Guna2GroupBox1.Controls.Add(Me.Label2)
        Me.Guna2GroupBox1.Controls.Add(Me.cmbAccount)
        Me.Guna2GroupBox1.Controls.Add(Me.dtpExpenseDate)
        Me.Guna2GroupBox1.Controls.Add(Me.txtExpenseID)
        Me.Guna2GroupBox1.Controls.Add(Me.txtExpenseDescription)
        Me.Guna2GroupBox1.Controls.Add(Me.txtExpenseAmount)
        Me.Guna2GroupBox1.Controls.Add(Me.txtExpenseName)
        Me.Guna2GroupBox1.CustomBorderColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2GroupBox1.Font = New System.Drawing.Font("FC Minimal", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2GroupBox1.ForeColor = System.Drawing.Color.White
        Me.Guna2GroupBox1.Location = New System.Drawing.Point(12, 461)
        Me.Guna2GroupBox1.Name = "Guna2GroupBox1"
        Me.Guna2GroupBox1.Size = New System.Drawing.Size(1876, 221)
        Me.Guna2GroupBox1.TabIndex = 36
        Me.Guna2GroupBox1.Text = "รายจ่าย"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.White
        Me.Label12.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(1403, 71)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(55, 32)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "บาท"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.White
        Me.Label3.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(1057, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 32)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "คำอธิบาย :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.White
        Me.Label7.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(1050, 71)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(131, 32)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "จำนวนเงิน :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(581, 164)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 32)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "บัญชี :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(559, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 32)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "ชื่อผู้รับ :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(68, 160)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 32)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "วันที่ทำรายการ :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(86, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(158, 32)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "เลขที่รายจ่าย :"
        '
        'cmbAccount
        '
        Me.cmbAccount.BackColor = System.Drawing.Color.Transparent
        Me.cmbAccount.BorderRadius = 5
        Me.cmbAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cmbAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAccount.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbAccount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbAccount.Font = New System.Drawing.Font("FC Minimal", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAccount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.cmbAccount.ItemHeight = 30
        Me.cmbAccount.Location = New System.Drawing.Point(666, 160)
        Me.cmbAccount.Name = "cmbAccount"
        Me.cmbAccount.Size = New System.Drawing.Size(208, 36)
        Me.cmbAccount.TabIndex = 6
        '
        'dtpExpenseDate
        '
        Me.dtpExpenseDate.Animated = True
        Me.dtpExpenseDate.BackColor = System.Drawing.Color.White
        Me.dtpExpenseDate.BorderRadius = 5
        Me.dtpExpenseDate.Checked = True
        Me.dtpExpenseDate.FillColor = System.Drawing.Color.DarkSlateBlue
        Me.dtpExpenseDate.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpExpenseDate.ForeColor = System.Drawing.Color.White
        Me.dtpExpenseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpExpenseDate.Location = New System.Drawing.Point(250, 152)
        Me.dtpExpenseDate.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpExpenseDate.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpExpenseDate.Name = "dtpExpenseDate"
        Me.dtpExpenseDate.Size = New System.Drawing.Size(208, 48)
        Me.dtpExpenseDate.TabIndex = 5
        Me.dtpExpenseDate.Value = New Date(2024, 9, 7, 22, 10, 28, 596)
        '
        'txtExpenseID
        '
        Me.txtExpenseID.Animated = True
        Me.txtExpenseID.BackColor = System.Drawing.Color.White
        Me.txtExpenseID.BorderRadius = 10
        Me.txtExpenseID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpenseID.DefaultText = ""
        Me.txtExpenseID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtExpenseID.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtExpenseID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseID.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpenseID.ForeColor = System.Drawing.Color.Black
        Me.txtExpenseID.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseID.Location = New System.Drawing.Point(251, 65)
        Me.txtExpenseID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtExpenseID.Name = "txtExpenseID"
        Me.txtExpenseID.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtExpenseID.PlaceholderText = ""
        Me.txtExpenseID.ReadOnly = True
        Me.txtExpenseID.SelectedText = ""
        Me.txtExpenseID.Size = New System.Drawing.Size(208, 48)
        Me.txtExpenseID.TabIndex = 0
        '
        'txtExpenseDescription
        '
        Me.txtExpenseDescription.Animated = True
        Me.txtExpenseDescription.BackColor = System.Drawing.Color.White
        Me.txtExpenseDescription.BorderRadius = 10
        Me.txtExpenseDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpenseDescription.DefaultText = ""
        Me.txtExpenseDescription.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtExpenseDescription.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtExpenseDescription.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseDescription.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseDescription.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseDescription.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpenseDescription.ForeColor = System.Drawing.Color.Black
        Me.txtExpenseDescription.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseDescription.Location = New System.Drawing.Point(1188, 152)
        Me.txtExpenseDescription.Margin = New System.Windows.Forms.Padding(4)
        Me.txtExpenseDescription.Name = "txtExpenseDescription"
        Me.txtExpenseDescription.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtExpenseDescription.PlaceholderText = ""
        Me.txtExpenseDescription.SelectedText = ""
        Me.txtExpenseDescription.Size = New System.Drawing.Size(345, 48)
        Me.txtExpenseDescription.TabIndex = 0
        '
        'txtExpenseAmount
        '
        Me.txtExpenseAmount.Animated = True
        Me.txtExpenseAmount.BackColor = System.Drawing.Color.White
        Me.txtExpenseAmount.BorderRadius = 10
        Me.txtExpenseAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpenseAmount.DefaultText = ""
        Me.txtExpenseAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtExpenseAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtExpenseAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseAmount.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpenseAmount.ForeColor = System.Drawing.Color.Black
        Me.txtExpenseAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseAmount.Location = New System.Drawing.Point(1188, 65)
        Me.txtExpenseAmount.Margin = New System.Windows.Forms.Padding(4)
        Me.txtExpenseAmount.Name = "txtExpenseAmount"
        Me.txtExpenseAmount.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtExpenseAmount.PlaceholderText = ""
        Me.txtExpenseAmount.SelectedText = ""
        Me.txtExpenseAmount.Size = New System.Drawing.Size(208, 48)
        Me.txtExpenseAmount.TabIndex = 0
        Me.txtExpenseAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtExpenseName
        '
        Me.txtExpenseName.Animated = True
        Me.txtExpenseName.BackColor = System.Drawing.Color.White
        Me.txtExpenseName.BorderRadius = 10
        Me.txtExpenseName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpenseName.DefaultText = ""
        Me.txtExpenseName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtExpenseName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtExpenseName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpenseName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseName.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpenseName.ForeColor = System.Drawing.Color.Black
        Me.txtExpenseName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpenseName.Location = New System.Drawing.Point(666, 65)
        Me.txtExpenseName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtExpenseName.Name = "txtExpenseName"
        Me.txtExpenseName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtExpenseName.PlaceholderText = ""
        Me.txtExpenseName.SelectedText = ""
        Me.txtExpenseName.Size = New System.Drawing.Size(208, 48)
        Me.txtExpenseName.TabIndex = 0
        '
        'dgvExpenseDetails
        '
        Me.dgvExpenseDetails.AllowUserToAddRows = False
        Me.dgvExpenseDetails.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvExpenseDetails.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvExpenseDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvExpenseDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvExpenseDetails.ColumnHeadersHeight = 70
        Me.dgvExpenseDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvExpenseDetails.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgvExpenseDetails.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.Location = New System.Drawing.Point(968, 113)
        Me.dgvExpenseDetails.Name = "dgvExpenseDetails"
        Me.dgvExpenseDetails.ReadOnly = True
        Me.dgvExpenseDetails.RowHeadersVisible = False
        Me.dgvExpenseDetails.Size = New System.Drawing.Size(920, 342)
        Me.dgvExpenseDetails.TabIndex = 34
        Me.dgvExpenseDetails.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.dgvExpenseDetails.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.dgvExpenseDetails.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.dgvExpenseDetails.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.dgvExpenseDetails.ThemeStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.Height = 70
        Me.dgvExpenseDetails.ThemeStyle.ReadOnly = True
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.Height = 22
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2Panel1.Controls.Add(Me.Guna2ControlBox1)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(1900, 33)
        Me.Guna2Panel1.TabIndex = 40
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.BackColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.CustomIconSize = 30.0!
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(1850, 1)
        Me.Guna2ControlBox1.Name = "Guna2ControlBox1"
        Me.Guna2ControlBox1.Size = New System.Drawing.Size(38, 29)
        Me.Guna2ControlBox1.TabIndex = 5
        '
        'Guna2HtmlLabel11
        '
        Me.Guna2HtmlLabel11.BackColor = System.Drawing.Color.White
        Me.Guna2HtmlLabel11.Font = New System.Drawing.Font("FC Minimal", 36.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel11.Location = New System.Drawing.Point(12, 50)
        Me.Guna2HtmlLabel11.Name = "Guna2HtmlLabel11"
        Me.Guna2HtmlLabel11.Size = New System.Drawing.Size(218, 50)
        Me.Guna2HtmlLabel11.TabIndex = 41
        Me.Guna2HtmlLabel11.Text = "จัดการรายจ่าย"
        '
        'txtSearch
        '
        Me.txtSearch.Animated = True
        Me.txtSearch.BorderRadius = 5
        Me.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearch.DefaultText = ""
        Me.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSearch.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSearch.Location = New System.Drawing.Point(1645, 64)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtSearch.PlaceholderText = "เลขที่รายจ่าย"
        Me.txtSearch.SelectedText = ""
        Me.txtSearch.Size = New System.Drawing.Size(243, 42)
        Me.txtSearch.TabIndex = 42
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.White
        Me.Label11.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(1569, 69)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 32)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "ค้นหา"
        '
        'frmEditExpense
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1900, 980)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.Guna2HtmlLabel11)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Guna2GroupBox2)
        Me.Controls.Add(Me.dgvExpenses)
        Me.Controls.Add(Me.Guna2GroupBox1)
        Me.Controls.Add(Me.dgvExpenseDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmEditExpense"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmEditExpense"
        Me.Guna2GroupBox2.ResumeLayout(False)
        Me.Guna2GroupBox2.PerformLayout()
        CType(Me.dgvExpenses, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2GroupBox1.ResumeLayout(False)
        Me.Guna2GroupBox1.PerformLayout()
        CType(Me.dgvExpenseDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Guna2Elipse1 As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents btnPrint As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnDelete As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnSave As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2GroupBox2 As Guna.UI2.WinForms.Guna2GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtDetailAmount As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDetailName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDetailID As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents dgvExpenses As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Guna2GroupBox1 As Guna.UI2.WinForms.Guna2GroupBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbAccount As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents dtpExpenseDate As Guna.UI2.WinForms.Guna2DateTimePicker
    Friend WithEvents txtExpenseID As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtExpenseDescription As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtExpenseAmount As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtExpenseName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents dgvExpenseDetails As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Guna2HtmlLabel11 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label11 As Label
End Class
