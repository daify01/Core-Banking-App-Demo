<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AddAdmin.aspx.cs" Inherits="CBAPractice.Start.AddAdmin" %>
<!DOCTYPE html>

<html>
     <!-- Content Header (Page header) -->
   <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Add Admin</title>
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
    <!-- iCheck -->
    <link rel="stylesheet" href="../plugins/iCheck/square/blue.css">
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

      <!-- Main content -->
       <body>
          <div class="row">
            <!-- left column -->
            <div class="col-md-6" style="width: 100%">
              <!-- general form elements -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Enter Admin Details</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNameFName">FirstName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameFName" title="Firstnames" pattern="[A-Za-z]*" maxlength="20" required="" placeholder="Enter Name"/>
                               
                            </div>
                             <div class="form-group">
                                 <label for="TextBoxNameLName">LastName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameLName" title="LastName" pattern="[A-Za-z]*" maxlength="20" required="" placeholder="Enter Name"/>
                                
                            </div>
                            <div class="form-group">
                                <label for="TextBoxNameONames">OtherNames</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameONames" title="OtherName" maxlength="20" placeholder="Enter Name"/>
                                
                            </div>
                            
                                </div>
                            
                            <div class="col-md-6">
                            <div class="form-group">
                                <label for="TextBoxNameEmail">Email</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameEmail" title="Enter proper email format" pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$" maxlength="80" required="" placeholder="Enter Email"/>
                             
                            </div>
                            
                            
                            <div class="form-group">
                                <asp:Label ID="Role" runat="server" Font-Bold="true">Role</asp:Label>
                                <asp:DropDownList ID="DropDownRole" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem/>
                                </asp:DropDownList>
                               
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNameUName">UserName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameUName" title="Username:letters and numbers only" pattern="[A-Za-z0-9]*" maxlength="30" required="" placeholder="Enter Username"/>
                               
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNamePhone">PhoneNumber</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNamePhone" title="11 digit Phone Number, no spacing" pattern="[0-9][0-9]{10,10}" maxlength="15" required="" placeholder="Enter Phone"/>
                              
                            </div>

                        

                        <div class="box-footer">
                            <input type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" id="searchsubmit" class="btn btn-flat btn-primary pull-right" name="Add"/>
                           
                        </div>
                                </div>
                            </div>

                    </form>
                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->
        </div>
        <!-- /.row -->
    </body>
    
      <!-- jQuery 2.1.4 -->
    <script src="../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="../bootstrap/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="../plugins/iCheck/icheck.min.js"></script>
      <!-- AdminLTE App -->
    <script src="../dist/js/app.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../dist/js/demo.js"></script>
     <script type="text/javascript">
         $(function () {
             
             $('#DropDownRole').select2({ placeholder: "Please Select A Role" });
         });
    </script>
    </html>

