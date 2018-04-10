<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ChangeUserPassword.aspx.cs" Inherits="CBAPractice.ManageUsers.ChangeUserPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>User Management
            <small>Change User Password</small>
        </h1>
    </section>

      <!-- Main content -->
        <section class="content">
          <div class="row">
            <!-- left column -->
            <div class="col-md-6" style="width: 100%">
              <!-- general form elements -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Change Password</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNamePassword">Old Password</label>
                        <input type="password" runat="server" class="form-control" id="TextBoxNamePassword" title="Former Password" maxlength="200" required="" placeholder="Enter Name"/>
                               
                            </div>
                             <div class="form-group">
                                 <label for="TextBoxNameNewPassword">New Password</label>
                        <input type="password" runat="server" class="form-control" id="TextBoxNameNewPassword" title="At least 8 characters, with one special character and one digit" pattern="^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$" maxlength="20" required="" placeholder="Enter Name"/>
                                
                            </div>
                            <div class="form-group">
                                <label for="TextBoxNameConfNewPassword">Confirm New Password</label>
                        <input type="password" runat="server" class="form-control" id="TextBoxNameConfNewPassword" title="At least 8 characters, with one special character and one digit" pattern="^.*(?=.{8,})(?=.*[\d])(?=.*[\W]).*$" maxlength="20" placeholder="Enter Name"/>
                                
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
    </section>
    <!-- /.content -->
</asp:Content>
