<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Progress
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
        Me.AddingPokemon_Bar = New System.Windows.Forms.ProgressBar()
        Me.Information = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'AddingPokemon_Bar
        '
        Me.AddingPokemon_Bar.Location = New System.Drawing.Point(21, 40)
        Me.AddingPokemon_Bar.Name = "AddingPokemon_Bar"
        Me.AddingPokemon_Bar.Size = New System.Drawing.Size(251, 46)
        Me.AddingPokemon_Bar.TabIndex = 0
        '
        'Information
        '
        Me.Information.AutoSize = True
        Me.Information.Font = New System.Drawing.Font("Lucida Bright", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Information.Location = New System.Drawing.Point(113, 117)
        Me.Information.Name = "Information"
        Me.Information.Size = New System.Drawing.Size(59, 18)
        Me.Information.TabIndex = 1
        Me.Information.Text = "Label1"
        '
        'Progress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 215)
        Me.Controls.Add(Me.Information)
        Me.Controls.Add(Me.AddingPokemon_Bar)
        Me.Name = "Progress"
        Me.Text = "Adding Pokemon"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddingPokemon_Bar As System.Windows.Forms.ProgressBar
    Friend WithEvents Information As System.Windows.Forms.Label
End Class
