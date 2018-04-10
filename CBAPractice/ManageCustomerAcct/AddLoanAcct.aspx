<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddLoanAcct.aspx.cs" Inherits="CBAPractice.ManageCustomerAcct.AddLoanAcct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Loan Account Management
            <small>Add Loan Account</small>
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
                        <h3 class="box-title">Add Loan Account</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                            <input type="hidden" runat="server" id="TextBoxId" />
                            <div class="form-group">
                                <label for="TextBoxNameCustAcctNo">Customer Account Number</label>
                                <input type="text" runat="server" class="form-control" id="TextBoxNameCustAcctNo" title="Account Number" pattern="[0-9]*" maxlength="150" required="required" placeholder="Enter A/C No" />
                                <button id="Button3" type="submit" runat="server" onserverclick="verifysubmit_OnServerClick" class="btn btn-primary">Verify Account</button>
                            </div>

                            <div class="form-group">
                                <label for="TextBoxNameCustAcctName">Customer Account Name</label>
                                <input type="text" runat="server" class="form-control" id="TextBoxNameCustAcctName" title="Account Name" pattern="^([a-zA-Z_][a-zA-Z_ ]*[a-zA-Z_]$" maxlength="150" required="required" placeholder="Enter A/C Name" />

                            </div>

                            <div class="form-group">
                                <asp:Label ID="LoanAccount" runat="server" Font-Bold="true">Loan Account</asp:Label>
                                <asp:DropDownList ID="DropDownListLoanAccount" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem/>
                                </asp:DropDownList>

                            </div>
                                </div>
                            
                            <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="PaymentSchedule" runat="server" Font-Bold="true">Payment Schedule</asp:Label>
                                <asp:DropDownList ID="DropDownListPaymentSchedule" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem/>
                                </asp:DropDownList>
                                <label for="TextBoxNameNumberOfDays">NumberOfDays</label>
                                <input type="text" runat="server" class="form-control" id="TextBoxNameNumberOfDays" title="Integer values Only" maxlength="3" placeholder="Enter Number of Days" />
                            </div>

                            <div class="form-group">
                                <label for="TextBoxNameAmount">Loan Amount(Naira)</label>
                                <input type="text" runat="server" class="form-control" id="TextBoxNameAmount" title="Loan Amount" pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,2})?$)" maxlength="15" required="required" placeholder="Enter Amount" />

                            </div>

                            <div class="form-group">
                                <label for="TextBoxNameDuration">Loan Duration(Days)</label>
                                <input type="text" runat="server" class="form-control" id="TextBoxNameDuration" title="Loan Period" pattern="(^(\+|\-)(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{1,2})?$)" maxlength="150" required="required" placeholder="Enter Loan Duration" />

                            </div>



                            <div class="box-footer">
                                <input type="submit" runat="server" onserverclick="searchsubmit_OnServerClick" id="searchsubmit" class="btn btn-flat btn-primary pull-right" name="Add" />

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
            $('#DropDownListLoanAccount').select2({ placeholder: "Please Select A Loan Account" }),
            $('#DropDownListPaymentSchedule').select2({ placeholder: "Please Select A Payment Schedule" });
        });
        $('#<%= DropDownListPaymentSchedule.ClientID %>').change(function () {
            if (this.value == 'Days') {
                $('#<%= TextBoxNameNumberOfDays.ClientID %>').css('display', 'block');
            }
            else {
                $('#<%= TextBoxNameNumberOfDays.ClientID %>').css('display', 'none');
                
            }
        });
    </script>


</asp:Content>




