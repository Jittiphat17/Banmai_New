<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebtor
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
        Me.ReportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.drpYear = New System.Windows.Forms.ComboBox()
        Me.drpMonth = New System.Windows.Forms.ComboBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.AccountBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Dbbanmai1DataSetBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Db_banmai1DataSet = New Banmai.db_banmai1DataSet()
        Me.AccountTableAdapter = New Banmai.db_banmai1DataSetTableAdapters.AccountTableAdapter()
        CType(Me.AccountBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Dbbanmai1DataSetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Db_banmai1DataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ReportViewer1
        '
        Me.ReportViewer1.LocalReport.ReportEmbeddedResource = "Banmai.Individualdebtor.rdlc"
        Me.ReportViewer1.Location = New System.Drawing.Point(12, 85)
        Me.ReportViewer1.Name = "ReportViewer1"
        Me.ReportViewer1.ServerReport.BearerToken = Nothing
        Me.ReportViewer1.Size = New System.Drawing.Size(1081, 482)
        Me.ReportViewer1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(647, 49)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 37
        Me.Button1.Text = "ค้นหา"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(477, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(14, 13)
        Me.Label3.TabIndex = 36
        Me.Label3.Text = "ปี"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(245, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "เดือน"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "ชื่อประเภทบัญชี"
        '
        'drpYear
        '
        Me.drpYear.FormattingEnabled = True
        Me.drpYear.Location = New System.Drawing.Point(497, 51)
        Me.drpYear.Name = "drpYear"
        Me.drpYear.Size = New System.Drawing.Size(121, 21)
        Me.drpYear.TabIndex = 33
        '
        'drpMonth
        '
        Me.drpMonth.FormattingEnabled = True
        Me.drpMonth.Location = New System.Drawing.Point(331, 51)
        Me.drpMonth.Name = "drpMonth"
        Me.drpMonth.Size = New System.Drawing.Size(121, 21)
        Me.drpMonth.TabIndex = 32
        '
        'ComboBox1
        '
        Me.ComboBox1.DataSource = Me.AccountBindingSource
        Me.ComboBox1.DisplayMember = "acc_name"
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(98, 49)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 31
        Me.ComboBox1.ValueMember = "acc_id"
        '
        'AccountBindingSource
        '
        Me.AccountBindingSource.DataMember = "Account"
        Me.AccountBindingSource.DataSource = Me.Dbbanmai1DataSetBindingSource
        '
        'Dbbanmai1DataSetBindingSource
        '
        Me.Dbbanmai1DataSetBindingSource.DataSource = Me.Db_banmai1DataSet
        Me.Dbbanmai1DataSetBindingSource.Position = 0
        '
        'Db_banmai1DataSet
        '
        Me.Db_banmai1DataSet.DataSetName = "db_banmai1DataSet"
        Me.Db_banmai1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AccountTableAdapter
        '
        Me.AccountTableAdapter.ClearBeforeFill = True
        '
        'frmDebtor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1105, 579)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.drpYear)
        Me.Controls.Add(Me.drpMonth)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.ReportViewer1)
        Me.Name = "frmDebtor"
        Me.Text = "frmDebtor"
        CType(Me.AccountBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Dbbanmai1DataSetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Db_banmai1DataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ReportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents Button1 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents drpYear As ComboBox
    Friend WithEvents drpMonth As ComboBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Db_banmai1DataSet As db_banmai1DataSet
    Friend WithEvents AccountBindingSource As BindingSource
    Friend WithEvents Dbbanmai1DataSetBindingSource As BindingSource
    Friend WithEvents AccountTableAdapter As db_banmai1DataSetTableAdapters.AccountTableAdapter
End Class
