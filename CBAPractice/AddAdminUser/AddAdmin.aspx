<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddAdmin.aspx.cs" Inherits="CBAPractice.AddAdminUser.AddAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Admin User
            <small>Add/Edit Admin</small>
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
                        <input type="text" runat="server" class="form-control" id="TextBoxNameONames" title="OtherName" pattern="" maxlength="20" placeholder="Enter Name"/>
                                
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
    </section>
    <!-- /.content -->
     <script type="text/javascript">
         $(function () {
             
             $('#DropDownRole').select2({ placeholder: "Please Select A Role" });
         });
    </script>
</asp:Content>
