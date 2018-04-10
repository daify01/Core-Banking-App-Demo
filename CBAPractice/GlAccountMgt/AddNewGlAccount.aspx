<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddNewGlAccount.aspx.cs" Inherits="CBAPractice.GlAccountMgt.AddNewGlAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>GL Account Management
            <small>Add/Edit Gl Account</small>
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
                  <h3 class="box-title">Enter GL Account Details</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            
                             <div class="form-group">
                                <asp:Label ID="GlCategory" runat="server" Font-Bold="true">Gl Category</asp:Label>
                               <asp:DropDownList ID="DropDownGlCategory" class="form-group" ClientIDMode="Static" runat="server" required="required" AppendDataBoundItems="True" CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>

                            <div class="form-group">
                                <label for="TextBoxNameGlAccountName">Gl Account Name</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameGlAccountName" title="GlAccount Name" pattern="^[a-zA-Z_]+( [a-zA-Z_]+)*$" maxlength="150" required="" placeholder="Enter Gl Category Name"/>
                                
                            </div>
                          
                          <div class="form-group">
                                <asp:Label ID="Branch" runat="server" Font-Bold="true">Branch</asp:Label>
                               <asp:DropDownList ID="DropDownBranch" class="form-group" ClientIDMode="Static" runat="server" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList> 
                            

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
            $('#DropDownGlCategory').select2({placeholder: "Please Select A Category"}),
            $('#DropDownBranch').select2({placeholder: "Please Select A Category"});
        });
    </script>


</asp:Content>
