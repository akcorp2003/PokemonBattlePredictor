<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class test
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
        Me.Initiate_build = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Initiate_build
        '
        Me.Initiate_build.Location = New System.Drawing.Point(47, 69)
        Me.Initiate_build.Name = "Initiate_build"
        Me.Initiate_build.Size = New System.Drawing.Size(182, 23)
        Me.Initiate_build.TabIndex = 0
        Me.Initiate_build.Text = "Build Pokemon Dictionary"
        Me.Initiate_build.UseVisualStyleBackColor = True
        '
        'test
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.Initiate_build)
        Me.Name = "test"
        Me.Text = "Test Cases"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Initiate_build As System.Windows.Forms.Button
End Class
