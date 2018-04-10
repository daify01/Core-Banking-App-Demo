<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewAllCustomerAcct.aspx.cs" Inherits="CBAPractice.ManageCustomerAcct.ViewAllCustomerAcct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <!-- Main content -->
     <section class="content-header">
        <h1>Customer Account Management
           
            <small>View Customer Accounts</small>
        </h1>
    </section>
        <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">List of Customer Accounts</h3>
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
                              <label for="searchname">Account Type</label>
                              <input ClientIDMode="Static" id="searchtype" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Branch</label>
                              <input ClientIDMode="Static" id="searchbranch" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchaddress">Account Number</label>
                              <input id="searchaddress" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right">Submit</button>
                      </div> 
                  </div>
                </div>
    
                <div class="box-body">
                  <table id="viewcustomers" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                        <th></th>
                        <th>Account Number</th>
                        <th>Account Name</th>
                          <th>Account Type</th>
                          <th>Balance</th>
                        <th></th>
                          <th></th>
                          <th></th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                        <%--<th></th>
                        <th>Account Number</th>
                        <th>Account Name</th>
                          <th>Account Type</th>
                          <th>Balance</th>
                        <th></th>
                          <th></th>
                          <th></th>--%>
                      </tr>
                    </tfoot>
                  </table>
                </div><!-- /.box-body -->
              </div><!-- /.box -->
                </div>
              </div>
            </section>
   
    <div class="modal fade" id="ViewCustomerDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewCustomerDetailsModal" aria-hidden="true">
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
                      <td>Account Number</td>
                      <td id="detailAcctNo">
                      </td>
                    </tr>
                        <tr>
                      <td>2.</td>
                      <td>Account Name</td>
                      <td id="detailAcctName">
                      </td>
                    </tr>
                        <tr>
                      <td>3.</td>
                      <td>Account Type</td>
                      <td id="detailAcctType">
                      </td>
                    </tr>
                        <tr>
                      <td>4.</td>
                      <td>Balance</td>
                      <td id="AcctBalance">
                      </td>
                    </tr>
                        <tr>
                      <td>5.</td>
                      <td>Branch</td>
                      <td id="Branch">
                      </td>
                    </tr>
                        <tr>
                      <td>6.</td>
                      <td>CoT Charge</td>
                      <td id="CoTCharge">
                      </td>
                    </tr>
                  </tbody></table>
                
            </div>
            <div class="modal-footer">
                <button type="button" id="Close" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" id="Edit" class="btn btn-primary">Edit Account</button>
            </div>
        </div>
    </div>
</div>


    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewcustomers").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/CustomerAccountForView', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchName = $('#searchname').val();
                        d.searchType = $('#searchtype').val();
                        d.searchBranch = $('#searchbranch').val();
                        d.searchAddress = $('#searchaddress').val();
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
                { data: 'ACCountNumber' },
                    { data: 'ACCountName' },
                    { data: 'ACCountType' },
                    { data: 'BAlance' },
                    { data: 'ViewDetails' },
                    { data: 'CloseAccount' },
                    { data: 'OpenAccount' }
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

        //function Redirect(id) {
        //    window.location = '../ManageCustomerAcct/EditCustomerAcct.aspx?id=' + $(this).attr("data-id",id);
        //}


        function viewDetails(id) {
            $('#ViewCustomerDetailsModal').modal('show');
            $.ajax({
                url: '/api/Query/CustomerAccountDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#detailsContent").html(result);
                    $("#detailAcctNo").html(result.AccountNumber);
                    $('#detailAcctName').html(result.AccountName);
                    $('#detailAcctType').html(result.AccountTypeString);
                    $('#AcctBalance').html(result.Balance);
                    $('#Branch').html(result.Branch.BranchName);
                    $('#CoTCharge').html(result.CoTCharge);
                    $("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
        }

        $("#Edit").click(function () {
            location = '../ManageCustomerAcct/EditCustomerAccountReal.aspx?id=' + $(this).attr("data-id");
        });

        function closeAccount(id) {
            alertify.confirm("Do you want to close this account?",
                function () {
                    $.ajax({
                        url: '/api/Query/CloseAccount?id=' + id,
                        method: 'POST',
                        success: function (result) {
                            debugger;
                            alertify.alert("This user's account has been closed");
                             $('.status-' + id).html('Closed').addClass('btn-danger').removeClass('btn-info');
                            //$("input[type=button]").attr('disabled', 'disabled');
                            //$("button[type=button]").removeAttr('disabled');
                             location.reload();
                        },
                        error: function () {
                            debugger;
                        }
                        //error: alertify.alert("This user's account could not be closed")
                    });
                });
        }

        function openAccount(id) {
            alertify.confirm("Do you want to open this account?",
                function () {
                    $.ajax({
                        url: '/api/Query/OpenAccount?id=' + id,
                        method: 'POST',
                        success: function (result) {
                            debugger;
                            alertify.alert("This user's account has been opened");
                            $('.status-' + id).html('Open').addClass('btn-danger').removeClass('btn-info');
                            $("button[type=button]").removeAttr('disabled');
                            $("input[type=button]").attr('disabled', 'disabled');
                            location.reload();
                        },
                        error: function () {
                            debugger;
                        }
                        //error: alertify.alert("This user's account could not be closed")
                    });
                });
        }

        

        
        
        


    </script>




</asp:Content>


