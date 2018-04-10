<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="PostTransactionsIntoCustomerAcct.aspx.cs" Inherits="CBAPractice.TellerPostingMgt.PostTransactionsIntoCustomerAcct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
  
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Teller Postings
            <small>Post Transaction</small>
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
                  <h3 class="box-title">Post Transaction</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNameCustAcctNo">Customer Account Number</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameCustAcctNo" title="Customer Account Number" pattern="[0-9]*" maxlength="15" required="required" placeholder="Enter A/C No"/>
                                <button id="Button3" type="submit" runat="server" OnServerClick="verifysubmit_OnServerClick" class="btn btn-primary">Verify Account</button>
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNameCustAcctName">Customer Account Name</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameCustAcctName" title="Customer Account Name" pattern="^([a-zA-Z_][a-zA-Z_ ]*[a-zA-Z_]$" maxlength="150" required="required" placeholder="Enter A/C Name"/>
                                
                            </div>
                            
                             <div class="form-group">
                                <label for="TextBoxNameTillAcctName">Till Account Name</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameTillAcctName" title="Till Account Name" pattern="[A-Za-z]*" maxlength="150" required="required" placeholder="Enter A/C Name"/>
                                
                            </div>
                                </div>
                            
                             <div class="col-md-6">
                            <div class="form-group">
                                <label for="TextBoxNameAmount">Amount(Naira)</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameAmount" title="Amount" pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,2})?$)" maxlength="30" required="required" placeholder="Enter Amount"/>
                                
                            </div>
                            
                             <div class="form-group">
                                <asp:Label ID="PostingType" runat="server" Font-Bold="true">Posting Type</asp:Label>
                               <asp:DropDownList ID="DropDownListPostingType" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True"  CssClass="form-control" >
                                   <asp:ListItem/>
                               </asp:DropDownList>
                             
                            </div>
                            
                            <div class="form-group">
                                <label for="TextBoxNameNarration"> Narration</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameNarration" title="Narration" pattern="^[a-zA-Z0-9_]+( [a-zA-Z0-9_]+)*$" maxlength="150" required="required" placeholder="Enter Debit Narration"/>
                                
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
             $('#DropDownListPostingType').select2({ placeholder: "Please Select A Category" });
         });
    </script>
</asp:Content>




