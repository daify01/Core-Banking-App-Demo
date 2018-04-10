<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewListOfUsers.aspx.cs" Inherits="CBAPractice.ManageUsers.ViewListOfUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <!-- Main content -->
     <section class="content-header">
        <h1>User Management
           
            <small>View Users</small>
        </h1>
    </section>
        <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">List of Users</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-4">
                          <div class="form-group">
                              <label for="searchname">First Name</label>
                              <input ClientIDMode="Static" id="searchfname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <div class="col-md-4">
                          <div class="form-group">
                              <label for="searchname">Last Name</label>
                              <input ClientIDMode="Static" id="searchlname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-4">
                          <div class="form-group">
                              <label for="searchbranch">Branch Name</label>
                              <input id="searchbranch" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right" OnServerClick="searchsubmit_OnServerClick">Submit</button>
                      </div> 
                  </div>
                </div>
    
                <div class="box-body">
                  <table id="viewusers" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                        <th></th>
                        <th>FirstName</th>
                        <th>LastName</th>
                          <th>User Role</th>
                        <th>Branch</th>
                          <th></th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                        <%--<th></th>
                        <th>FirstName</th>
                        <th>LastName</th>
                          <th>User Role</th>
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
    <!-- Modal -->
<div class="modal fade" id="ViewUserDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewUserDetailsModal" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">User Details</h4>
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
                      <td>BranchName</td>
                      <td id="detailBName">
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
                      <td>UserName</td>
                      <td id="detailUName">
                      </td>
                    </tr>
                        <tr>
                      <td>6.</td>
                      <td>Role</td>
                      <td id="detailRole">
                      </td>
                    </tr>
                        <tr>
                      <td>7.</td>
                      <td>PhoneNumber</td>
                      <td id="detailPhone">
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
</div>
    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewusers").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/Users', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchFName = $('#searchfname').val();
                        d.searchLName = $('#searchlname').val();
                        d.searchBranch = $('#searchbranch').val();
                    }
                },
                "searching": false,
                "fnRowCallback": function(nRow, aData, iDisplayIndex) {
                    var oSettings = table.fnSettings();
                    $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);
                    return nRow;
                },
                columns: [
                    { data: 'ID' },
                { data: 'Fname' },
                    { data: 'LName' },
                    { data: 'URole' },
                    { data: 'BRanch' },
                    { data: 'ViewDetails' }
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
        function viewDetails(id) {
            $('#ViewUserDetailsModal').modal('show');
            $.ajax({
                url: '/api/Query/UserDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#detailsContent").html(result);
                    $('#detailFName').html(result.FirstName);
                    $('#detailLName').html(result.LastName);
                    $('#detailBName').html(result.Branch.BranchName);
                    $('#detailEmail').html(result.Email);
                    $('#detailUName').html(result.UserName);
                    $('#detailRole').html(result.RoleString);
                    $('#detailPhone').html(result.PhoneNumber);
                    $("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
        }
        $("#Edit").click(function () {
            location = '../ManageUsers/AddNewUser.aspx?id=' + $(this).attr("data-id");
        });
        //$('#ViewDetailsModal').on('hidden.bs.modal', function() {
        //    if (!($("#ViewDetailsModal").data('bs.modal') || {}).isShown) {
        //        $("#Edit").show();
        //        $("#Close").show();
        //    }
        //});
    </script>
</asp:Content>
