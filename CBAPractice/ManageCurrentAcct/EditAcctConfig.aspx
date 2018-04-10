<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="EditAcctConfig.aspx.cs" Inherits="CBAPractice.ManageCurrentAcct.EditAcctConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Current Account Configuration
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
                  <h3 class="box-title">Configure Current Account</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            
                            <div class="form-group">
                                <label for="TextBoxNameCreditInterestRate">Credit Interest Rate (%)</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameCreditInterestRate" title="Enter Interest Rate" pattern="[0-9]*" maxlength="3" required="required" placeholder="Enter Credit Interest Rate (%)"/>
                                
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNameMinimumBalance">Minimum Balance</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameMinimumBalance" title="Enter Minimum Balance" pattern="[0-9]*" maxlength="4" required="required" placeholder="Enter Minumum Balance"/>
                                
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="CurrentGL" runat="server" Font-Bold="true">Current GL Acct</asp:Label>
                               <asp:DropDownList ID="DropDownCurrentGL" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                                </div>
                                </div>
                                
                             <div class="col-md-6">
                                <div class="form-group">
                                <label for="TextBoxNameCOT">COT</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameCOT" title="Enter COT" pattern="[0-9]*" maxlength="150" required="required" placeholder="Enter COT Amount"/>
                                
                            </div>
                                
                               <div class="form-group">
                                <asp:Label ID="COTIncomeGL" runat="server" Font-Bold="true">COT Income GL Acct</asp:Label>
                               <asp:DropDownList ID="DropDownCOTIncomeGL" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                            
                             <div class="form-group">
                                <asp:Label ID="GlCategory" runat="server" Font-Bold="true">Interest Expense GL Acct</asp:Label>
                               <asp:DropDownList ID="DropDownGlCategory" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem Text="Select" Value="0"/>
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
            $('#DropDownCurrentGL').select2({ placeholder: "Please Select A Category" }),
            $('#DropDownCOTIncomeGL').select2({ placeholder: "Please Select A Category" }),
            $('#DropDownGlCategory').select2({ placeholder: "Please Select A Category" });
        });
    </script>



</asp:Content>


