<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TeamBlue_3 = New System.Windows.Forms.PictureBox()
        Me.TeamBlue_2 = New System.Windows.Forms.PictureBox()
        Me.TeamBlue_1 = New System.Windows.Forms.PictureBox()
        Me.TeamRed_3 = New System.Windows.Forms.PictureBox()
        Me.BattleButton = New System.Windows.Forms.Button()
        Me.TeamRed_2 = New System.Windows.Forms.PictureBox()
        Me.TeamRed_1 = New System.Windows.Forms.PictureBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Pokedex_Writer = New System.ComponentModel.BackgroundWorker()
        Me.Pokedex_Formatter = New System.ComponentModel.BackgroundWorker()
        Me.Worker_InsertURI = New System.ComponentModel.BackgroundWorker()
        CType(Me.TeamBlue_3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TeamBlue_2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TeamBlue_1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TeamRed_3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TeamRed_2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TeamRed_1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Comic Sans MS", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(116, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(347, 38)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Pokemon Battle Simulator!"
        '
        'TeamBlue_3
        '
        Me.TeamBlue_3.BackgroundImage = CType(resources.GetObject("TeamBlue_3.BackgroundImage"), System.Drawing.Image)
        Me.TeamBlue_3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamBlue_3.Location = New System.Drawing.Point(426, 187)
        Me.TeamBlue_3.Name = "TeamBlue_3"
        Me.TeamBlue_3.Size = New System.Drawing.Size(78, 63)
        Me.TeamBlue_3.TabIndex = 1
        Me.TeamBlue_3.TabStop = False
        '
        'TeamBlue_2
        '
        Me.TeamBlue_2.BackgroundImage = CType(resources.GetObject("TeamBlue_2.BackgroundImage"), System.Drawing.Image)
        Me.TeamBlue_2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamBlue_2.Location = New System.Drawing.Point(262, 187)
        Me.TeamBlue_2.Name = "TeamBlue_2"
        Me.TeamBlue_2.Size = New System.Drawing.Size(78, 63)
        Me.TeamBlue_2.TabIndex = 2
        Me.TeamBlue_2.TabStop = False
        '
        'TeamBlue_1
        '
        Me.TeamBlue_1.BackgroundImage = CType(resources.GetObject("TeamBlue_1.BackgroundImage"), System.Drawing.Image)
        Me.TeamBlue_1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamBlue_1.Location = New System.Drawing.Point(87, 187)
        Me.TeamBlue_1.Name = "TeamBlue_1"
        Me.TeamBlue_1.Size = New System.Drawing.Size(78, 63)
        Me.TeamBlue_1.TabIndex = 3
        Me.TeamBlue_1.TabStop = False
        '
        'TeamRed_3
        '
        Me.TeamRed_3.BackgroundImage = CType(resources.GetObject("TeamRed_3.BackgroundImage"), System.Drawing.Image)
        Me.TeamRed_3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamRed_3.Location = New System.Drawing.Point(426, 357)
        Me.TeamRed_3.Name = "TeamRed_3"
        Me.TeamRed_3.Size = New System.Drawing.Size(78, 63)
        Me.TeamRed_3.TabIndex = 4
        Me.TeamRed_3.TabStop = False
        '
        'BattleButton
        '
        Me.BattleButton.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BattleButton.Location = New System.Drawing.Point(157, 76)
        Me.BattleButton.Name = "BattleButton"
        Me.BattleButton.Size = New System.Drawing.Size(272, 54)
        Me.BattleButton.TabIndex = 5
        Me.BattleButton.Text = "Enter the Battling Pokemon!"
        Me.BattleButton.UseVisualStyleBackColor = True
        '
        'TeamRed_2
        '
        Me.TeamRed_2.BackgroundImage = CType(resources.GetObject("TeamRed_2.BackgroundImage"), System.Drawing.Image)
        Me.TeamRed_2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamRed_2.Location = New System.Drawing.Point(260, 357)
        Me.TeamRed_2.Name = "TeamRed_2"
        Me.TeamRed_2.Size = New System.Drawing.Size(80, 63)
        Me.TeamRed_2.TabIndex = 6
        Me.TeamRed_2.TabStop = False
        '
        'TeamRed_1
        '
        Me.TeamRed_1.BackgroundImage = CType(resources.GetObject("TeamRed_1.BackgroundImage"), System.Drawing.Image)
        Me.TeamRed_1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TeamRed_1.Location = New System.Drawing.Point(87, 357)
        Me.TeamRed_1.Name = "TeamRed_1"
        Me.TeamRed_1.Size = New System.Drawing.Size(78, 63)
        Me.TeamRed_1.TabIndex = 7
        Me.TeamRed_1.TabStop = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Button2.Font = New System.Drawing.Font("Comic Sans MS", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(188, 506)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(214, 60)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "PREDICT!"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Wide Latin", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(257, 295)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 29)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "VS."
        '
        'Pokedex_Writer
        '
        Me.Pokedex_Writer.WorkerReportsProgress = True
        Me.Pokedex_Writer.WorkerSupportsCancellation = True
        '
        'Pokedex_Formatter
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(630, 587)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TeamRed_1)
        Me.Controls.Add(Me.TeamRed_2)
        Me.Controls.Add(Me.BattleButton)
        Me.Controls.Add(Me.TeamRed_3)
        Me.Controls.Add(Me.TeamBlue_1)
        Me.Controls.Add(Me.TeamBlue_2)
        Me.Controls.Add(Me.TeamBlue_3)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.TeamBlue_3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TeamBlue_2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TeamBlue_1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TeamRed_3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TeamRed_2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TeamRed_1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TeamBlue_3 As System.Windows.Forms.PictureBox
    Friend WithEvents TeamBlue_2 As System.Windows.Forms.PictureBox
    Friend WithEvents TeamBlue_1 As System.Windows.Forms.PictureBox
    Friend WithEvents TeamRed_3 As System.Windows.Forms.PictureBox
    Friend WithEvents BattleButton As System.Windows.Forms.Button
    Friend WithEvents TeamRed_2 As System.Windows.Forms.PictureBox
    Friend WithEvents TeamRed_1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Pokedex_Writer As System.ComponentModel.BackgroundWorker
    Friend WithEvents Pokedex_Formatter As System.ComponentModel.BackgroundWorker
    Friend WithEvents Worker_InsertURI As System.ComponentModel.BackgroundWorker

End Class
