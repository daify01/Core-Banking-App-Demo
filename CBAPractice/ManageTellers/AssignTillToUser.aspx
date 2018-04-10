<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AssignTillToUser.aspx.cs" Inherits="CBAPractice.ManageTellers.AssignTillToUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Teller Management
            <small>Assign Till to User</small>
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
                  <h3 class="box-title">Assign Till to User</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                           
                            
                             <div class="form-group">
                                <asp:Label ID="User" runat="server" Font-Bold="true">User</asp:Label>
                               <asp:DropDownList ID="DropDownUser" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                            
                              <div class="form-group">
                                <asp:Label ID="TillAccount" runat="server" Font-Bold="true">Till Account</asp:Label>
                               <asp:DropDownList ID="DropDownTillAccount" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/> 
                               </asp:DropDownList>
                             
                            </div>

                            

                        <div class="box-footer">
                            <input type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" id="searchsubmit" class="btn btn-flat btn-primary pull-right" name="Add"/>
                           
                        </div>
                     </div>
                </div>
                <!-- /.box -->
                        </form>

            </div>
            <!--/.col (right) -->
        </div>
        <!-- /.row -->
              </div>
    </section>
    <!-- /.content -->


    <script type="text/javascript">
        $(function () {
            $('#DropDownUser').select2({ placeholder: "Please Select A User" }),
            $('#DropDownTillAccount').select2({ placeholder: "Please Select A Till Account" });
        });
    </script>




</asp:Content>


