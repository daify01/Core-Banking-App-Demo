<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="PostTransactionsIntoGLs.aspx.cs" Inherits="CBAPractice.GlPostingMgt.PostTransactionsIntoGLs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>GL Postings
            <small>Post Transactions Into GLs</small>
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
                  <h3 class="box-title">Post Transactions</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            
                             <div class="form-group">
                                <asp:Label ID="GlAcctToDebit" runat="server" Font-Bold="true">GL Account To Debit</asp:Label>
                               <asp:DropDownList ID="DropDownListGlAcctToDebit" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNameDebitNarration">Debit Narration</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameDebitNarration" title="Debit Narration" pattern="^[a-zA-Z0-9_]+( [a-zA-Z0-9_]+)*$" maxlength="150" required="required" placeholder="Enter Debit Narration"/>
                                
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="GlAcctToCredit" runat="server" Font-Bold="true">GL Account To Credit</asp:Label>
                               <asp:DropDownList ID="DropDownListGlAcctToCredit" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                                </div>
                            
                            <div class="col-md-6">
                            <div class="form-group">
                                <label for="TextBoxNameCreditNarration">Credit Narration</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameCreditNarration" title="Credit Narration" pattern="^[a-zA-Z0-9_]+( [a-zA-Z0-9_]+)*$" maxlength="150" required="required" placeholder="Enter Credit Narration"/>
                                
                            </div>
                            
                             <div class="form-group">
                                <label for="TextBoxNameDebitOrCreditAmnt">Amount(Naira)</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameDebitOrCreditAmnt" title="Amount" pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,2})?$)" maxlength="150" required="required" placeholder="Enter Amount"/>
                                
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
            $('#DropDownListGlAcctToDebit').select2({ placeholder: "Please Select A Category" }),
            $('#DropDownListGlAcctToCredit').select2({ placeholder: "Please Select A Category" });
        });
    </script>
</asp:Content>


