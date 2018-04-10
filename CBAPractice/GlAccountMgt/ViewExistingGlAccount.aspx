﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewExistingGlAccount.aspx.cs" Inherits="CBAPractice.GlAccountMgt.ViewExistingGlAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
     <!-- Main content -->
     <section class="content-header">
        <h1>GL Account Management
           
            <small>View GL Accounts</small>
        </h1>
    </section>
    
    <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">List of Gl Accounts</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Gl Account Code</label>
                              <input ClientIDMode="Static" id="searchglacctcode" class="form-control" name="name" type="text" runat="server"/>
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
                              <label for="searchglacct">Gl Category</label>
                              <input id="searchglacct" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right">Submit</button>
                      </div> 
                  </div>
                </div>
    
                <div class="box-body">
                  <table id="viewusers" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                        <th></th>
                        <th>GL Account Name</th>
                          <th>GL Account Code</th>
                          <th>GL Category</th>
                          <th>Branch</th>
                        <th>Balance</th>
                       <th></th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                       <%-- <th></th>
                        < <th>GL Account Name</th>
                          <th>GL Account Code</th>
                          <th>GL Category</th>
                          <th>Bramch</th>
                        <th>Balance</th>
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
<%--<div class="modal fade" id="ViewUserDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewUserDetailsModal" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">GL Category Details</h4>
            </div>
            <div class="modal-body" id="detailsContent" style="overflow-y:scroll; max-height:450px">

                <table class="table table-bordered">
                    <tbody>
                    <tr>
                      <td>1.</td>
                      <td>GlCategory Name</td>
                      <td id="detailGlCategoryName">
                      </td>
                    </tr>
                        <tr>
                      <td>2.</td>
                      <td>Main Account Category</td>
                      <td id="detailMainAcctCategory">
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
            var table = $("#viewusers").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/GlAccount', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchGLName = $('#searchname').val();
                        d.searchGLCode = $('#searchglacctcode').val();
                        d.searchGLBranch = $('#searchbranch').val();
                        d.searchGlAcctCategory = $('#searchglacct').val();
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
                { data: 'AccountName' },
                    { data: 'AccountCode' },
                    { data: 'Category' },
                    { data: 'Branch' },
                    { data: 'BAlance' },
                    { data: 'Edit' }
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
        //    $('#ViewUserDetailsModal').modal('show');
        //    $.ajax({
        //        url: '/api/Query/GlCategoryDetails?id=' + id,
        //        method: 'POST',
        //        success: function (result) {
        //            //$("#detailsContent").html(result);
        //            $('#detailGlCategoryName').html(result.GlCategoryName);
        //            $('#detailMainAcctCategory').html(result.MainAccountCategoryString);
        //            $("#Edit").attr("data-id", id);
        //        },
        //        error: function (xhr, status, error) {
        //            $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
        //        }
        //    });
        //}
        function edit(id) {
            $.ajax({
                url: '/api/Query/GlAccountDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#Edit").click(function () {
                    location = '../GlAccountMgt/AddNewGlAccount.aspx?id=' + id ;
                    //$(this).attr("data-id")
                    //$("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
        }
        //$("#Edit").click(function () {
        //    location = '../GlCategoryMgt/AddNewGlCategory.aspx?id=' + $(this).attr("data-id");
        //});
        //$('#ViewDetailsModal').on('hidden.bs.modal', function() {
        //    if (!($("#ViewDetailsModal").data('bs.modal') || {}).isShown) {
        //        $("#Edit").show();
        //        $("#Close").show();
        //    }
        //});
    </script>

</asp:Content>


