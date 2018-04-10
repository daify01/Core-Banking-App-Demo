<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewLoanAccounts.aspx.cs" Inherits="CBAPractice.ManageCustomerAcct.ViewLoanAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
     <!-- Main content -->
     <section class="content-header">
        <h1>Customer Account Management
           
            <small>View Loan Accounts</small>
        </h1>
    </section>
        <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">List of Loan Accounts</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Account Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Loan Duration</label>
                              <input ClientIDMode="Static" id="searchlduration" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Payment Schedule</label>
                              <input ClientIDMode="Static" id="searchptype" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchbranch">Account Number</label>
                              <input id="searchbranch" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right" OnServerClick="searchsubmit_OnServerClick">Submit</button>
                      </div> 
                  </div>
                </div>
    
                <div class="box-body">
                  <table id="viewcustomers" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                        <th></th>
                        <th>Loan Account Name</th>
                          <th>Account Number</th>
                          <th>Linked Account Type</th>
                        <th>LoanAmount</th>
                          <th>Loan Duration</th>
                          <th>Interest</th>
                        <th>Balance Unpaid</th>
                          <th>Loan StartDate</th>
                          <th>Loan DueDate</th>
                          <th>Payment Schedule</th>
                          <th>No Of Days</th>
                          <th>Status</th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                        <%--<th></th>
                       <th>LoanAccountName</th>
                          <th>AccountNumber</th>
                        <th>LoanAmount</th>
                          <th>LoanDuration</th>
                          <th>Interest</th>
                        <th>Balance</th>
                          <th>LoanStartDate</th>
                          <th>LoanDueDate</th>--%>
                      </tr>
                    </tfoot>
                  </table>
                    <form role="form" runat="server" ClientIDMode="Static" id="form1" >
                    <div class="text-bold text-red">LOAN OVERDUE ACCOUNTS (NOT FULLY PAID BEFORE DUE DATE)</div>
                    <%--DataKeyNames="ID--%>
                    <asp:GridView ID="LoanOverdue" runat="server" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" 
                OnPageIndexChanging="LoanOverdue_PageIndexChanging" EnableViewState="False">
  <Columns>
    <asp:BoundField DataField="AccountNumber" HeaderText="AccountNumber" ReadOnly="True" SortExpression="ProductName" />
    <asp:BoundField DataField="AccountName" HeaderText="AccountName" ReadOnly="True" SortExpression="CategoryName" />
      <asp:BoundField DataField="AccountType" HeaderText="Linked Account Type" ReadOnly="True" SortExpression="CategoryName" />
    <asp:BoundField DataField="LoanAmount" HeaderText="LoanAmount" ReadOnly="True" SortExpression="SupplierName" />
    <asp:BoundField DataField="LoanDuration" HeaderText="LoanDuration" ReadOnly="True" SortExpression="UnitPrice" />
    <asp:BoundField DataField="LoanInterest" HeaderText="LoanInterest" ReadOnly="True" SortExpression="Discontinued" />
    <asp:BoundField DataField="Balance" HeaderText="Balance Unpaid" ReadOnly="True" SortExpression="SupplierName" />
    <asp:BoundField DataField="LoanDueDate" HeaderText="LoanDueDate" ReadOnly="True" SortExpression="UnitPrice" />
  </Columns>
</asp:GridView>
                         </form>
                </div><!-- /.box-body -->
              </div><!-- /.box -->
                </div>
              </div>
            </section>
    <!-- Modal -->
<%--<div class="modal fade" id="ViewCustomerDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewCustomerDetailsModal" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Customer Details</h4>
            </div>
            <div class="modal-body" id="detailsContent" style="overflow-y:scroll; max-height:450px">

                <table class="table table-bordered">
                    <tbody>
                    <tr>
                      <td>1.</td>
                      <td>FirstName</td>
                      <td id="detailFName">
                      </td>
                    </tr>
                        <tr>
                      <td>2.</td>
                      <td>LastName</td>
                      <td id="detailLName">
                      </td>
                    </tr>
                    <tr>
                      <td>3.</td>
                      <td>OtherNames</td>
                      <td id="detailOName">
                      </td>
                    </tr>
                        <tr>
                      <td>4.</td>
                      <td>Email</td>
                      <td id="detailEmail">
                      </td>
                    </tr>
                        <tr>
                      <td>5.</td>
                      <td>Gender</td>
                      <td id="detailGender">
                      </td>
                    </tr>
                        <tr>
                      <td>6.</td>
                      <td>CustomerID</td>
                      <td id="detailCustomerID">
                      </td>
                    </tr>
                        <tr>
                      <td>7.</td>
                      <td>PhoneNumber</td>
                      <td id="detailPhoneNumber">
                      </td>
                    </tr>
                        <tr>
                      <td>8.</td>
                      <td>Address</td>
                      <td id="detailAddress">
                      </td>
                    </tr>
                  </tbody></table>

            </div>
            <div class="modal-footer">
                <button type="button" id="Close" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" id="Edit" class="btn btn-primary">Edit</button>
            </div>
        </div>
    </div>
</div>--%>
    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewcustomers").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/LoanAccounts', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchName = $('#searchname').val();
                        d.searchLDuration = $('#searchlduration').val();
                        d.searchPType = $('#searchptype').val();
                        d.searchBranch = $('#searchbranch').val();
                    }
                },
                "searching": false,
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    var oSettings = table.fnSettings();
                    $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);
                    return nRow;
                },
                columns: [
                    { data: 'ID' },
                { data: 'ACctName' },
                    { data: 'ACctNo' },
                    { data: 'AcctType' },
                    { data: 'AMnt' },
                    { data: 'Duration' },
                    { data: 'Interest' },
                    { data: 'BAlance' },
                    { data: 'StartDate' },
                    { data: 'DueDate' },
                { data: 'PaymentType' },
            { data: 'NoOFDays' },
                { data: 'Status' }
                ]
            });

            //table.on('order.dt search.dt', function () {
            //    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //        cell.innerHTML = i + 1;
            //    });
            //}).draw();

            $("#searchsubmit").click(function () {
                table.fnFilter(''); //.search('').draw();
            });
        });
        //function viewDetails(id) {
        //    $('#ViewCustomerDetailsModal').modal('show');
        //    $.ajax({
        //        url: '/api/Query/CustomerDetails?id=' + id,
        //        method: 'POST',
        //        success: function (result) {
        //            //$("#detailsContent").html(result);
        //            $('#detailFName').html(result.FirstName);
        //            $('#detailLName').html(result.LastName);
        //            $('#detailOName').html(result.OtherNames);
        //            $('#detailEmail').html(result.Email);
        //            $('#detailGender').html(result.GenderString);
        //            $('#detailCustomerID').html(result.Id);
        //            $('#detailPhoneNumber').html(result.PhoneNumber);
        //            $('#detailAddress').html(result.Address);
        //            $("#Edit").attr("data-id", id);
        //        },
        //        error: function (xhr, status, error) {
        //            $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
        //        }
        //    });
        //}
        //$("#Edit").click(function () {
        //    location = '../ManageCustomers/AddNewCustomer.aspx?id=' + $(this).attr("data-id");
        //});
        //$('#ViewDetailsModal').on('hidden.bs.modal', function() {
        //    if (!($("#ViewDetailsModal").data('bs.modal') || {}).isShown) {
        //        $("#Edit").show();
        //        $("#Close").show();
        //    }
        //});
    </script>


</asp:Content>


