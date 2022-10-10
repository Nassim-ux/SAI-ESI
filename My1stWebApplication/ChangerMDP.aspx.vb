Imports System.Data.SqlClient
Imports System.Configuration

Public Class ChangerMDP
    Inherits System.Web.UI.Page

    Dim con As SqlConnection
    Dim cmd2 As SqlCommand
    Dim table As String
    Dim password As String

    Dim Re As Integer
    Dim DeadLine As Date = #06/07/2019#

    Dim edit As Integer = 0


    Dim test As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	' Cette procedure s'execute à chaque rafraîchissement de la page 
        If Not (IsPostBack) Then
     'On vérifie si l'utilistaeur est connecté si ce n'est pas le cas il est renvoyé a la page de login 
            Dim cp As Integer = 0
            If (Session("user") = Nothing) Then
                Response.Redirect("LoginOfficiel.aspx")
            End If
			'Pour afficher le nom de l'enseignant en haut de la page
            Label1.Text = Session("name")
            Label2.Text = Label1.Text
        End If
		    'On affiche le type de l'utilisateur selon type en haut de la page
        If (Session("ENS")) Then
            If (Session("CodeEns") Like 0) Then
                labelEE.Text = "Personel"
            Else
                labelEE.Text = "Enseignant"
            End If
        Else
            labelEE.Text = "Etudiant"
        End If
    End Sub



    Private Sub Buttonlogout_Click(sender As Object, e As EventArgs) Handles Buttonlogout.Click
	 'Cette procedure sert a deconnecter l'utilisateur et vider les variables sessions
        Session.RemoveAll()
        Response.Redirect("LoginOfficiel.aspx")
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (Session("ENS")) Then
            table = "ENSEIGNANT"
            password = "Passwd"
        Else
            table = "ETUDIANTS"
            password = "PassWord"
        End If

        con = New SqlConnection("Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=saisiedenotes;Integrated Security=True")
        con.Open()

        If TextBox1.Text = Session("password") Then
          'On vérifer si le mot de pass entrée correspendant au le mot de pass du l'utilisteur stoqué dans une variable session
            If TextBox2.Text = TextBox3.Text Then
             'On vérifier si le nouveau mot de pass et la confirmation du nouveau mot de pass sont égaux
                If TextBox2.Text <> "" Then
                  'On fait l'insertion du nouveau mot de pass dans la BDD SQL
                    cmd2 = New SqlCommand("update " + table + " set " + password + "='" + TextBox2.Text + "' where NomUser='" + Session("user") + "'")
                    cmd2.Connection = con
                    cmd2.ExecuteNonQuery()
                    
                    Session("tef") = True
                    Session("password") = TextBox2.Text
                    If Session("ENS") Then
                        If (Session("CodeEns") Like 0) Then
                            Response.Redirect("PageAbsencesJustification.aspx")
                        Else
                            Response.Redirect("PageChoixEnseignant.aspx")
                        End If
                    Else
                            Response.Redirect("PageChoixEtudiant.aspx")
                    End If
                Else


                    
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alertx('VEUILLEZ ENTRER VOTRE NOUVEAU MOT DE PASSE !');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alertx('VEUILLEZ CONFIRMER VOTRE MOT DE PASSE');", True)
            End If

        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "alertx('MOT DE PASSE INCORRECT REESSAYER S IL VOUS PLAIT');", True)

        End If

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
	'Cette procedure sert a renvoyer l'utilisteur a sa page d'accueil quand il termine 
        If Session("ENS") Then
            If (Session("CodeEns") Like 0) Then
                Response.Redirect("PageAbsencesJustification.aspx")
            Else
                Response.Redirect("PageChoixEnseignant.aspx")
            End If
        Else
            Response.Redirect("PageChoixEtudiant.aspx")
        End If
    End Sub


End Class