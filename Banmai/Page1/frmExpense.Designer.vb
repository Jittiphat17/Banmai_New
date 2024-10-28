<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmExpense
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Guna2Elipse1 = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.btnPrintReceipt = New Guna.UI2.WinForms.Guna2Button()
        Me.btnCalculate = New Guna.UI2.WinForms.Guna2Button()
        Me.btnSave = New Guna.UI2.WinForms.Guna2Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTotalAmount = New System.Windows.Forms.Label()
        Me.dgvExpenseDetails = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2GroupBox1 = New Guna.UI2.WinForms.Guna2GroupBox()
        Me.Guna2HtmlLabel7 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel9 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel6 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel5 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel4 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel3 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.dtpBirth = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Me.cboDepositType = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtDetails = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtAmount = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtDescrip = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtMemberID = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtExpId = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2HtmlLabel8 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvExpenseDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2GroupBox1.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Elipse1
        '
        Me.Guna2Elipse1.TargetControl = Me
        '
        'btnPrintReceipt
        '
        Me.btnPrintReceipt.Animated = True
        Me.btnPrintReceipt.BorderRadius = 10
        Me.btnPrintReceipt.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnPrintReceipt.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnPrintReceipt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPrintReceipt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnPrintReceipt.FillColor = System.Drawing.Color.Gold
        Me.btnPrintReceipt.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintReceipt.ForeColor = System.Drawing.Color.White
        Me.btnPrintReceipt.Location = New System.Drawing.Point(943, 898)
        Me.btnPrintReceipt.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnPrintReceipt.Name = "btnPrintReceipt"
        Me.btnPrintReceipt.Size = New System.Drawing.Size(265, 70)
        Me.btnPrintReceipt.TabIndex = 37
        Me.btnPrintReceipt.Text = "พิมพ์"
        '
        'btnCalculate
        '
        Me.btnCalculate.Animated = True
        Me.btnCalculate.BorderRadius = 10
        Me.btnCalculate.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnCalculate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnCalculate.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnCalculate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnCalculate.FillColor = System.Drawing.Color.ForestGreen
        Me.btnCalculate.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold)
        Me.btnCalculate.ForeColor = System.Drawing.Color.White
        Me.btnCalculate.Location = New System.Drawing.Point(671, 898)
        Me.btnCalculate.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnCalculate.Name = "btnCalculate"
        Me.btnCalculate.Size = New System.Drawing.Size(265, 70)
        Me.btnCalculate.TabIndex = 38
        Me.btnCalculate.Text = "คำนวณ"
        '
        'btnSave
        '
        Me.btnSave.Animated = True
        Me.btnSave.BorderRadius = 10
        Me.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray
        Me.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray
        Me.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer), CType(CType(141, Byte), Integer))
        Me.btnSave.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(399, 898)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(265, 70)
        Me.btnSave.TabIndex = 39
        Me.btnSave.Text = "บันทึก"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Panel1.Controls.Add(Me.lblTotalAmount)
        Me.Panel1.Location = New System.Drawing.Point(1663, 898)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(205, 70)
        Me.Panel1.TabIndex = 36
        '
        'lblTotalAmount
        '
        Me.lblTotalAmount.AutoSize = True
        Me.lblTotalAmount.Font = New System.Drawing.Font("FC Minimal", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblTotalAmount.Location = New System.Drawing.Point(54, 18)
        Me.lblTotalAmount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalAmount.Name = "lblTotalAmount"
        Me.lblTotalAmount.Size = New System.Drawing.Size(75, 36)
        Me.lblTotalAmount.TabIndex = 10
        Me.lblTotalAmount.Text = "0.00"
        '
        'dgvExpenseDetails
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvExpenseDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Symbol", 8.25!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvExpenseDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvExpenseDetails.ColumnHeadersHeight = 50
        Me.dgvExpenseDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Symbol", 8.25!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvExpenseDetails.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvExpenseDetails.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.Location = New System.Drawing.Point(749, 120)
        Me.dgvExpenseDetails.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dgvExpenseDetails.Name = "dgvExpenseDetails"
        Me.dgvExpenseDetails.RowHeadersVisible = False
        Me.dgvExpenseDetails.Size = New System.Drawing.Size(1119, 772)
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
        Me.dgvExpenseDetails.ThemeStyle.HeaderStyle.Height = 50
        Me.dgvExpenseDetails.ThemeStyle.ReadOnly = False
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.Height = 22
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dgvExpenseDetails.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'Guna2GroupBox1
        '
        Me.Guna2GroupBox1.BackColor = System.Drawing.Color.White
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel7)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel9)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel6)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel5)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel4)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel3)
        Me.Guna2GroupBox1.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Guna2GroupBox1.Controls.Add(Me.dtpBirth)
        Me.Guna2GroupBox1.Controls.Add(Me.cboDepositType)
        Me.Guna2GroupBox1.Controls.Add(Me.txtDetails)
        Me.Guna2GroupBox1.Controls.Add(Me.txtAmount)
        Me.Guna2GroupBox1.Controls.Add(Me.txtDescrip)
        Me.Guna2GroupBox1.Controls.Add(Me.txtMemberID)
        Me.Guna2GroupBox1.CustomBorderColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2GroupBox1.Font = New System.Drawing.Font("FC Minimal", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2GroupBox1.ForeColor = System.Drawing.Color.White
        Me.Guna2GroupBox1.Location = New System.Drawing.Point(13, 120)
        Me.Guna2GroupBox1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2GroupBox1.Name = "Guna2GroupBox1"
        Me.Guna2GroupBox1.Size = New System.Drawing.Size(728, 772)
        Me.Guna2GroupBox1.TabIndex = 33
        Me.Guna2GroupBox1.Text = "ข้อมูล"
        Me.Guna2GroupBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Guna2HtmlLabel7
        '
        Me.Guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel7.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel7.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel7.Location = New System.Drawing.Point(141, 582)
        Me.Guna2HtmlLabel7.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel7.Name = "Guna2HtmlLabel7"
        Me.Guna2HtmlLabel7.Size = New System.Drawing.Size(47, 34)
        Me.Guna2HtmlLabel7.TabIndex = 2
        Me.Guna2HtmlLabel7.Text = "วันที่"
        '
        'Guna2HtmlLabel9
        '
        Me.Guna2HtmlLabel9.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel9.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel9.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel9.Location = New System.Drawing.Point(590, 503)
        Me.Guna2HtmlLabel9.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel9.Name = "Guna2HtmlLabel9"
        Me.Guna2HtmlLabel9.Size = New System.Drawing.Size(44, 34)
        Me.Guna2HtmlLabel9.TabIndex = 2
        Me.Guna2HtmlLabel9.Text = "บาท"
        '
        'Guna2HtmlLabel6
        '
        Me.Guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel6.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel6.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel6.Location = New System.Drawing.Point(70, 503)
        Me.Guna2HtmlLabel6.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel6.Name = "Guna2HtmlLabel6"
        Me.Guna2HtmlLabel6.Size = New System.Drawing.Size(106, 34)
        Me.Guna2HtmlLabel6.TabIndex = 2
        Me.Guna2HtmlLabel6.Text = "จำนวนเงิน"
        '
        'Guna2HtmlLabel5
        '
        Me.Guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel5.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel5.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel5.Location = New System.Drawing.Point(133, 432)
        Me.Guna2HtmlLabel5.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel5.Name = "Guna2HtmlLabel5"
        Me.Guna2HtmlLabel5.Size = New System.Drawing.Size(54, 34)
        Me.Guna2HtmlLabel5.TabIndex = 2
        Me.Guna2HtmlLabel5.Text = "บัญชี"
        '
        'Guna2HtmlLabel4
        '
        Me.Guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel4.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel4.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel4.Location = New System.Drawing.Point(80, 364)
        Me.Guna2HtmlLabel4.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel4.Name = "Guna2HtmlLabel4"
        Me.Guna2HtmlLabel4.Size = New System.Drawing.Size(99, 34)
        Me.Guna2HtmlLabel4.TabIndex = 2
        Me.Guna2HtmlLabel4.Text = "คำอธิบาย"
        '
        'Guna2HtmlLabel3
        '
        Me.Guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel3.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel3.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel3.Location = New System.Drawing.Point(59, 235)
        Me.Guna2HtmlLabel3.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel3.Name = "Guna2HtmlLabel3"
        Me.Guna2HtmlLabel3.Size = New System.Drawing.Size(116, 34)
        Me.Guna2HtmlLabel3.TabIndex = 2
        Me.Guna2HtmlLabel3.Text = "รายละเอียด"
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel2.ForeColor = System.Drawing.Color.Black
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(140, 126)
        Me.Guna2HtmlLabel2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(46, 34)
        Me.Guna2HtmlLabel2.TabIndex = 2
        Me.Guna2HtmlLabel2.Text = "ผู้รับ"
        '
        'dtpBirth
        '
        Me.dtpBirth.BorderRadius = 10
        Me.dtpBirth.Checked = True
        Me.dtpBirth.FillColor = System.Drawing.Color.DarkSlateBlue
        Me.dtpBirth.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpBirth.ForeColor = System.Drawing.Color.White
        Me.dtpBirth.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBirth.Location = New System.Drawing.Point(203, 573)
        Me.dtpBirth.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.dtpBirth.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBirth.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBirth.Name = "dtpBirth"
        Me.dtpBirth.Size = New System.Drawing.Size(300, 52)
        Me.dtpBirth.TabIndex = 4
        Me.dtpBirth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.dtpBirth.Value = New Date(2024, 9, 24, 21, 12, 23, 746)
        '
        'cboDepositType
        '
        Me.cboDepositType.BackColor = System.Drawing.Color.Transparent
        Me.cboDepositType.BorderRadius = 5
        Me.cboDepositType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboDepositType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDepositType.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cboDepositType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cboDepositType.Font = New System.Drawing.Font("FC Minimal", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDepositType.ForeColor = System.Drawing.Color.Black
        Me.cboDepositType.ItemHeight = 30
        Me.cboDepositType.Location = New System.Drawing.Point(203, 430)
        Me.cboDepositType.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cboDepositType.Name = "cboDepositType"
        Me.cboDepositType.Size = New System.Drawing.Size(262, 36)
        Me.cboDepositType.TabIndex = 3
        '
        'txtDetails
        '
        Me.txtDetails.BackColor = System.Drawing.Color.White
        Me.txtDetails.BorderRadius = 5
        Me.txtDetails.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDetails.DefaultText = ""
        Me.txtDetails.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDetails.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDetails.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetails.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDetails.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetails.Font = New System.Drawing.Font("FC Minimal", 20.25!)
        Me.txtDetails.ForeColor = System.Drawing.Color.Black
        Me.txtDetails.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDetails.Location = New System.Drawing.Point(203, 195)
        Me.txtDetails.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtDetails.PlaceholderText = ""
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtDetails.SelectedText = ""
        Me.txtDetails.Size = New System.Drawing.Size(401, 128)
        Me.txtDetails.TabIndex = 2
        '
        'txtAmount
        '
        Me.txtAmount.BackColor = System.Drawing.Color.White
        Me.txtAmount.BorderRadius = 5
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.DefaultText = ""
        Me.txtAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAmount.Font = New System.Drawing.Font("FC Minimal", 24.0!)
        Me.txtAmount.ForeColor = System.Drawing.Color.Black
        Me.txtAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAmount.Location = New System.Drawing.Point(203, 489)
        Me.txtAmount.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtAmount.PlaceholderText = ""
        Me.txtAmount.SelectedText = ""
        Me.txtAmount.Size = New System.Drawing.Size(377, 61)
        Me.txtAmount.TabIndex = 2
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDescrip
        '
        Me.txtDescrip.BackColor = System.Drawing.Color.White
        Me.txtDescrip.BorderRadius = 10
        Me.txtDescrip.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescrip.DefaultText = ""
        Me.txtDescrip.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtDescrip.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtDescrip.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDescrip.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtDescrip.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDescrip.Font = New System.Drawing.Font("FC Minimal", 14.25!)
        Me.txtDescrip.ForeColor = System.Drawing.Color.Black
        Me.txtDescrip.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDescrip.Location = New System.Drawing.Point(203, 348)
        Me.txtDescrip.Margin = New System.Windows.Forms.Padding(6, 7, 6, 7)
        Me.txtDescrip.Name = "txtDescrip"
        Me.txtDescrip.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtDescrip.PlaceholderText = ""
        Me.txtDescrip.SelectedText = ""
        Me.txtDescrip.Size = New System.Drawing.Size(377, 61)
        Me.txtDescrip.TabIndex = 2
        '
        'txtMemberID
        '
        Me.txtMemberID.BackColor = System.Drawing.Color.White
        Me.txtMemberID.BorderRadius = 10
        Me.txtMemberID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemberID.DefaultText = ""
        Me.txtMemberID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMemberID.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMemberID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberID.Font = New System.Drawing.Font("FC Minimal", 24.0!)
        Me.txtMemberID.ForeColor = System.Drawing.Color.Black
        Me.txtMemberID.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberID.Location = New System.Drawing.Point(203, 111)
        Me.txtMemberID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMemberID.Name = "txtMemberID"
        Me.txtMemberID.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtMemberID.PlaceholderText = ""
        Me.txtMemberID.SelectedText = ""
        Me.txtMemberID.Size = New System.Drawing.Size(377, 61)
        Me.txtMemberID.TabIndex = 2
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("FC Minimal", 36.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(13, 61)
        Me.Guna2HtmlLabel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(118, 50)
        Me.Guna2HtmlLabel1.TabIndex = 32
        Me.Guna2HtmlLabel1.Text = "รายจ่าย"
        '
        'txtExpId
        '
        Me.txtExpId.Animated = True
        Me.txtExpId.BackColor = System.Drawing.Color.White
        Me.txtExpId.BorderRadius = 10
        Me.txtExpId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpId.DefaultText = ""
        Me.txtExpId.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtExpId.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtExpId.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpId.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtExpId.FillColor = System.Drawing.Color.LightGray
        Me.txtExpId.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpId.Font = New System.Drawing.Font("FC Minimal", 24.0!)
        Me.txtExpId.ForeColor = System.Drawing.Color.Black
        Me.txtExpId.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtExpId.Location = New System.Drawing.Point(1653, 50)
        Me.txtExpId.Margin = New System.Windows.Forms.Padding(4)
        Me.txtExpId.Name = "txtExpId"
        Me.txtExpId.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtExpId.PlaceholderText = ""
        Me.txtExpId.ReadOnly = True
        Me.txtExpId.SelectedText = ""
        Me.txtExpId.Size = New System.Drawing.Size(215, 61)
        Me.txtExpId.TabIndex = 35
        Me.txtExpId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Guna2HtmlLabel8
        '
        Me.Guna2HtmlLabel8.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel8.Font = New System.Drawing.Font("FC Minimal", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel8.Location = New System.Drawing.Point(1520, 64)
        Me.Guna2HtmlLabel8.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2HtmlLabel8.Name = "Guna2HtmlLabel8"
        Me.Guna2HtmlLabel8.Size = New System.Drawing.Size(125, 34)
        Me.Guna2HtmlLabel8.TabIndex = 32
        Me.Guna2HtmlLabel8.Text = "รหัสการจ่าย"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.BackColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.CustomIconSize = 30.0!
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(1841, 1)
        Me.Guna2ControlBox1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2ControlBox1.Name = "Guna2ControlBox1"
        Me.Guna2ControlBox1.Size = New System.Drawing.Size(44, 29)
        Me.Guna2ControlBox1.TabIndex = 5
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.DarkSlateBlue
        Me.Guna2Panel1.Controls.Add(Me.Guna2ControlBox1)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.Size = New System.Drawing.Size(1900, 33)
        Me.Guna2Panel1.TabIndex = 40
        '
        'frmExpense
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1900, 980)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.btnPrintReceipt)
        Me.Controls.Add(Me.btnCalculate)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.dgvExpenseDetails)
        Me.Controls.Add(Me.Guna2GroupBox1)
        Me.Controls.Add(Me.Guna2HtmlLabel8)
        Me.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Controls.Add(Me.txtExpId)
        Me.Font = New System.Drawing.Font("Symbol", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmExpense"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmExpense"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvExpenseDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2GroupBox1.ResumeLayout(False)
        Me.Guna2GroupBox1.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Guna2Elipse1 As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents btnPrintReceipt As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnCalculate As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnSave As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblTotalAmount As Label
    Friend WithEvents dgvExpenseDetails As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Guna2GroupBox1 As Guna.UI2.WinForms.Guna2GroupBox
    Friend WithEvents Guna2HtmlLabel7 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel6 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel5 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel4 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel3 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents dtpBirth As Guna.UI2.WinForms.Guna2DateTimePicker
    Friend WithEvents cboDepositType As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents txtDetails As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtAmount As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtDescrip As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtMemberID As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtExpId As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel8 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents PrintDialog1 As PrintDialog
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Guna2HtmlLabel9 As Guna.UI2.WinForms.Guna2HtmlLabel
End Class
