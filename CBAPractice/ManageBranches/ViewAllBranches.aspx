<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewAllBranches.aspx.cs" Inherits="CBAPractice.ManageBranches.ViewAllBranches" %>
<%@ Import Namespace="CBAPractice.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Main content -->
     <section class="content-header">
        <h1>Branch Management
           
            <small>View Branches</small>
        </h1>
    </section>
        <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">List of Bank's Branches</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchname">Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchcode">RC Code</label>
                              <input id="searchcode" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right" OnServerClick="searchsubmit_OnServerClick">Submit</button>
                      </div> 
                  </div>
                </div>
    
                <div class="box-body">
                  <table id="viewbranches" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                        <th></th>
                        <th>Name</th>
                        <th>RC Number</th>
                        <th></th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                       <%-- <th></th>
                        <th>Name</th>
                        <th>RC Number</th>
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
<div class="modal fade" id="ViewDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewDetailsModal" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Branch Details</h4>
            </div>
            <div class="modal-body" id="detailsContent" style="overflow-y:scroll; max-height:450px">

                <table class="table table-bordered">
                    <tbody>
                    <tr>
                      <td>1.</td>
                      <td>Name</td>
                      <td id="detailName">
                      </td>
                    </tr>
                    <tr>
                      <td>2.</td>
                      <td>RC Number</td>
                      <td id="detailRCNumber">
                      </td>
                    </tr>
                    <tr>
                      <td>3.</td>
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
</div>
    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewbranches").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/Branch', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchName = $('#searchname').val();
                        d.searchCode = $('#searchcode').val();
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
                    { data: 'BRANCHName' },
                    { data: 'RCNumber' },
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
            $('#ViewDetailsModal').modal('show');
            $.ajax({
                url: '/api/Query/BranchDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#detailsContent").html(result);
                    $('#detailName').html(result.BranchName);
                    $('#detailRCNumber').html(result.RcNumber);
                    $('#detailAddress').html(result.Address);
                    $("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
        }
        $("#Edit").click(function () {
            location = '../ManageBranches/AddNewBranch.aspx?id=' + $(this).attr("data-id");
        });
        //$('#ViewDetailsModal').on('hidden.bs.modal', function() {
        //    if (!($("#ViewDetailsModal").data('bs.modal') || {}).isShown) {
        //        $("#Edit").show();
        //        $("#Close").show();
        //    }
        //});
    </script>
</asp:Content>
