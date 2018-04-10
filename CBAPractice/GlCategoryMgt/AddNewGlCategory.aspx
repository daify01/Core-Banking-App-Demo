<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddNewGlCategory.aspx.cs" Inherits="CBAPractice.GlCategoryMgt.AddNewGlCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>GL Catgory Management
            <small>Add/Edit Gl Catgeory</small>
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
                  <h3 class="box-title">Enter User Details</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNameGlCategoryName">Gl Category Name</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameGlCategoryName" title="Gl Category Name" pattern="^[a-zA-Z_]+( [a-zA-Z_]+)*$" maxlength="150" required="" placeholder="Enter Gl Category Name"/>
                                
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="MainAccountCategory" runat="server" Font-Bold="true">Main Account Category</asp:Label>
                               <asp:DropDownList ID="DropDownMainAccountCategory" class="form-group" runat="server" ClientIDMode="Static" required="rquired" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                           
                            <div class="form-group">
                                <label for="TextBoxNameDescription">Description</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameDescription" title="Gl Category Description" pattern="^[a-zA-Z0-9_]+( [a-zA-Z0-9_]+)*$" maxlength="300" required="" placeholder="Description"/>
                               
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
            $('#DropDownMainAccountCategory').select2({ placeholder: "Please Select An Account Category" });
        });
    </script>
</asp:Content>
