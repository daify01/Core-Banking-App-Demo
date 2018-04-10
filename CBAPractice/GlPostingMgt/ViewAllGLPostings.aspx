<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="ViewAllGLPostings.aspx.cs" Inherits="CBAPractice.GlPostingMgt.ViewAllGLPostings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
     <!-- Main content -->
     <section class="content-header">
        <h1>GL Posting Management
           
            <small>View GL Postings</small>
        </h1>
    </section>
    
 <%--  <section class="box-header">
        <form>
 <button type="button"  id="reportAtDate" class="btn btn-flat btn-primary pull-right">Report At Date</button>
            <input type="date" id="date1" name="bday" class="pull-right">
              </form>
         </section>--%>
    
    <section class="content">
          
          <div class="row">
            <div class="col-xs-12">
              <div class="box">
                <div class="box-header">
                  <h3 class="box-title">Table of Gl Postings</h3>
                </div><!-- /.box-header -->
                <div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-4">
                          <div class="form-group">
                              <label for="searchname">Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                    <%--<div class="col-md-4">
                    <input type="date" id="date" name="bday" class="pull-right">
                        </div>--%>
                      <div class="col-md-4">
                          <div class="form-group">
                              <label for="searchglacct">GlAccount Code</label>
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
                          <th>Actual Date of Transaction</th>
                        <th>TransactionDate (Financial)</th>
                          <th>DebitAccountName</th>
                          <th>DebitAccountCode</th>
                          <th>CreditAccountName</th>
                        <th>CreditAccountCode</th>
                          <th>DebitNarration</th>
                          <th>CrebitNarration</th>
                        <th>Amount</th>
                      </tr>
                    </thead>
                        
                    <tfoot>
                      <tr>
                      <%--  <th></th>
                        <th>TransactionDate</th>
                          <th>DebitAccountName</th>
                          <th>DebitAccountCode</th>
                          <th>CreditAccountName</th>
                        <th>CreditAccountCode</th>
                          <th>DebitNarration</th>
                          <th>CrebitNarration</th>
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
                    "url": '/api/Query/GlPosting', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchGLName = $('#searchname').val();
                        d.searchGLCode = $('#searchglacct').val();
                        //d.searchDate = $('#date').val();
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
                    { data: 'ActualDate' },
                { data: 'TRansactionDate' },
                    { data: 'DebitAccountName' },
                    { data: 'DebitAccountCode' },
                    { data: 'CreditAccountName' },
                    { data: 'CreditAccountCode' },
                    { data: 'DEbitNarration' },
                { data: 'CReditNarration' },
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




