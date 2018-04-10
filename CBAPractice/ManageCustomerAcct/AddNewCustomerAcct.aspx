<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddNewCustomerAcct.aspx.cs" Inherits="CBAPractice.ManageCustomerAcct.AddNewCustomerAcct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
   <!-- Main content -->
     <section class="content-header">
        <h1>Customer Account Management
           
            <small>Add New Customer Accounts</small>
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
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchname">First Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchaddress">Last Name</label>
                              <input id="searchaddress" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
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
                        <th>FirstName</th>
                        <th>LastName</th>
                          <th>OtherNames</th>
                          <th>Email</th>
                          <th>Gender</th>
                        <th></th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                        <%--<th></th>
                        <th>FirstName</th>
                        <th>LastName</th>
                          <th>OtherNames</th>
                          <th>Email</th>
                          <th>Gender</th>
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
                
                Confirmation : Do You Really Want to Add this Account?
                
            </div>
            <div class="modal-footer">
                <button type="button" id="Close" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" id="Edit" class="btn btn-primary">Add</button>
            </div>
        </div>
    </div>
</div>


    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewcustomers").DataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/CustomerAccounts', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchName = $('#searchname').val();
                        d.searchAddress = $('#searchaddress').val();
                    }
                },
                "searching": false,
                columns: [
                    { data: 'ID' },
                { data: 'FIRSTName' },
                    { data: 'LASTName' },
                    { data: 'OTHERNames' },
                    { data: 'EMail' },
                    { data: 'GENder' },
                    { data: 'AddAccounts' }
                ]
            });

            table.on('order.dt search.dt', function () {
                table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            $("#searchsubmit").click(function () {
                table.search('').draw();
            });
        });

        //function Redirect(id) {
        //    window.location = '../ManageCustomerAcct/EditCustomerAcct.aspx?id=' + $(this).attr("data-id",id);
        //}


        function Redirect(id) {
            $('#ViewCustomerDetailsModal').modal('show');
            $.ajax({
                url: '/api/Query/CustomerDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#detailsContent").html(result);
                    //$('#detailFName').html(result.FirstName);
                    //$('#detailLName').html(result.LastName);
                    //$('#detailOName').html(result.OtherNames);
                    //$('#detailEmail').html(result.Email);
                    //$('#detailGender').html(result.GenderString);
                    //$('#detailCustomerID').html(result.Id);
                    //$('#detailPhoneNumber').html(result.PhoneNumber);
                    //$('#detailAddress').html(result.Address);
                    $("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
        }

        $("#Edit").click(function () {
            location = '../ManageCustomerAcct/EditCustomerAcct.aspx?id=' + $(this).attr("data-id");
        });

       
    </script>




</asp:Content>
