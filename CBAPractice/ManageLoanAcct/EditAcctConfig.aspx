<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="EditAcctConfig.aspx.cs" Inherits="CBAPractice.ManageLoanAcct.EditAcctConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
  
  
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Loan Account Configuration
            <small>Edit Account Configuration</small>
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
                  <h3 class="box-title">Configure Loan Account</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            
                            <div class="form-group">
                                <label for="TextBoxNameDebitInterestRate">Debit Interest Rate (%)</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameDebitInterestRate" title="Enter Interest Rate" pattern="[0-9]*" maxlength="3" required="required" placeholder="Enter Credit Interest Rate (%)"/>
                                
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="LoanPrincipalGL" runat="server" Font-Bold="true">Loan Principal GL Account</asp:Label>
                               <asp:DropDownList ID="DropDownLoanPrincipalGL" class="form-group" ClientIDMode="Static" runat="server"  required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                            
                             <div class="form-group">
                                <asp:Label ID="GlAccount" runat="server" Font-Bold="true">Interest Income GL Acct</asp:Label>
                               <asp:DropDownList ID="DropDownGlAccount" class="form-group" ClientIDMode="Static" runat="server"  required="required" AppendDataBoundItems="True"  CssClass="form-control" >
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
             $('#DropDownLoanPrincipalGL').select2({ placeholder: "Please Select A Branch" }),
             $('#DropDownGlAccount').select2({ placeholder: "Please Select A Role" });
         });
    </script>



</asp:Content>
 
  

