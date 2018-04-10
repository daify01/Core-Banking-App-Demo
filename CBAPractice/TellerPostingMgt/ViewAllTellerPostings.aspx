<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewAllTellerPostings.aspx.cs" Inherits="CBAPractice.TellerPostingMgt.ViewAllTellerPostings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
 
     <!-- Main content -->
     <section class="content-header">
        <h1>Teller Posting Management
           
            <small>View Teller Postings</small>
        </h1>
    </section>
    
    <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">Table of Gl Postings</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                    <div class="col-md-3">
                        <input type="date" id="dateValue" ClientIDMode="Static" runat="server" name="bday" class="pull-right"/>
                         </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Customer Account Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                     <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchname">Till Account Name</label>
                              <input ClientIDMode="Static" id="searchtill" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <label for="searchglacct">Customer Account Number</label>
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
                        <th>TransactionDate</th>
                          <th>CustomerAccountName</th>
                          <th>CustomerAccountNumber</th>
                          <th>CustomerTillName</th>
                        <th>PostingType</th>
                          <th>Narration</th>
                          <th>Amount</th>
                       
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                        <%--<th></th>
                        <th>TransactionDate</th>
                          <th>CustomerAccountName</th>
                          <th>CustomerAccountNumber</th>
                          <th>CustomerTillName</th>
                        <th>PostingType</th>
                          <th>Narration</th>
                          <th>Amount</th>--%>
                       </tr>
                    </tfoot>
                  </table>
                </div><!-- /.box-body -->
              </div><!-- /.box -->
                </div>
              </div>
            </section>    

  
    <script type="text/javascript">
        $(function () {
            debugger;
            var table = $("#viewusers").dataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/TellerPosting', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchCustomerAccountName = $('#searchname').val();
                        d.searchTillAccountName = $('#searchtill').val();
                        d.searchAccountNumber = $('#searchglacct').val();
                        d.searchDate = $('#dateValue').val();
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
                { data: 'TRansactionDate' },
                    { data: 'CustomerAccountName' },
                    { data: 'CustomerAccountNumber' },
                    { data: 'TillAccountName' },
                    { data: 'PostingType' },
                    { data: 'NArration' },
                    { data: 'AMount' }
            
                ]
            });
            debugger;

            //table.on('order.dt search.dt', function () {
            //    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            //        cell.innerHTML = i + 1;
            //    });
            //}).draw();

            $("#searchsubmit").click(function () {
                table.fnFilter(''); //.search('').draw();
            });
        });

    </script>

</asp:Content>





