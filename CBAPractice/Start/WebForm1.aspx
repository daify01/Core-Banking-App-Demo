<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs" Inherits="CBAPractice.Start.RecoverPassword" %>


<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Login Page</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.5 -->
    <link rel="stylesheet" href="../bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="../dist/css/AdminLTE.min.css">
    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link rel="stylesheet" href="../dist/css/skins/_all-skins.min.css">
    <!-- Alertify -->
    <script src="../dist/js/alertify.min.js"></script>
    <link rel="stylesheet" href="../dist/css/alertify.min.css"/>
    <link rel="stylesheet" href="../dist/css/themes/default.min.css"/>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
  </head>
  <body class="skin-blue sidebar-mini">
    <div class="wrapper">

      <!-- Content Wrapper. Contains page content -->
      <div class="content-wrapper" style="min-height: 929px;">
        <!-- Content Header (Page header) -->
        <section class="content-header">
          <h1>
            Password Recovery
            <small>Recover Password</small>
          </h1>
          <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i></a></li>
            <li><a href="#"></a></li>
            <li class="active"></li>
          </ol>
        </section>

        <div>
    <!-- Main content -->
       
          <div class="row">
            <div class="col-md-6">
              <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Recover Password</h3>
                </div><!-- /.box-header -->
                <!-- form start -->
                <form class="form-horizontal" id="form1" runat="server">
                  <div class="box-body">
                    <div class="form-group">
                      <label for="TextBoxNameUserName" class="col-sm-2 control-label">UserName</label>
                          <input type="text" runat="server" class="form-control" id="TextBoxNameUserName" placeholder="Username" required="required"/>
                      </div>
                    
                    <div class="form-group">
                      <label for="TextBoxNameEmail" class="col-sm-2 control-label">E-mail</label>
                      
                          <input type="password" runat="server" class="form-control" id="TextBoxNameEmail" TextMode="Password" placeholder="Password" required="required"/>
                      </div>
                   
                    <div class="form-group">
                        
                      </div>
                    </div>
                      <asp:Label ID="InvalidCredentialsMessage" runat="server" ForeColor="Red" Text="Your username or password is invalid. Please try again."
            Visible="False"></asp:Label> 
                 
                  <div class="box-footer">
                    <button id="Button1" type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" class="btn btn-primary pull-right">Recover Passoerd</button>
                  </div><!-- /.box-footer -->
                </form>
              </div><!-- /.box -->

            </div><!--/.col (right) -->
          </div>   <!-- /.row -->
       
      </div><!-- /.content-wrapper -->

    <!-- jQuery 2.1.4 -->
    <script src="../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->     <script src="../bootstrap/js/bootstrap.min.js"></script>
    <!-- FastClick -->
    <script src="../plugins/fastclick/fastclick.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../dist/js/app.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../dist/js/demo.js"></script>
      </div>
    </div>


  </body>
</html>
    
