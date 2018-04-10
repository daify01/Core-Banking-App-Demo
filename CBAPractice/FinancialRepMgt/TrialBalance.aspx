<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="TrialBalance.aspx.cs" Inherits="CBAPractice.FinancialRepMgt.TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Financial Report Management
            <small>View Trial Balance</small>
        </h1>
    </section>
    
    <section class="box-header">
        <form>
 <button type="button"  id="reportAtDate" class="btn btn-flat btn-primary pull-right">Report At Date</button>
            <input type="date" id="date" name="bday" class="pull-right">
              </form>
         </section>
      <!-- Main content -->
        <section class="content">
          <div class="row">
            <!-- left column -->
                    <div class="box">
                <div class="box-header">
                  <h3 id="title" class="box-title"></h3>
                </div><!-- /.box-header -->
                <div class="box-body no-padding">
                    
                  <table id="trialBalance" class="table table-striped">
                      <thead>
                      <tr>
                          <th>#</th>
                        <th>GL Account Name</th>
                        <th>Debit(₦)</th>
                        <th>Credit(₦)</th>
                      </tr>
                    </thead>
                      <tbody>
                          </tbody>
                  </table>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div>
            <!--/.col (right) -->
    </section>
    <!-- /.content -->
    
     <script type="text/javascript">
         $("#reportAtDate").click(function () {
             $.ajax({
                 url: '/api/Query/TrialBalanceContent',
                 method: 'POST',
                 data: { date : $('#date').val() },

                 success: function (data) {
                     debugger;
                     $('#title').html("Trial Balance as at " + $('#date').val());
                     var trialBalanceContent = '';
                     $('#trialBalance tbody').remove();
                     var debitSum = 0;
                     var creditSum = 0;
                     for (i = 0; i < data.length; i++) {

                         if ((data[i].Key.GlCategory.MainAccountCategory == "1") || (data[i].Key.GlCategory.MainAccountCategory == "5")) {
                             trialBalanceContent += '<tr><td>' + (i + 1) + '</td><td>' + data[i].Key.GlAccountName + '</td><td>' + data[i].Value + '</td><td></td></tr>';
                             debitSum += data[i].Value;
                         }
                         else if ((data[i].Key.GlCategory.MainAccountCategory == "2") || (data[i].Key.GlCategory.MainAccountCategory == "3") || (data[i].Key.GlCategory.MainAccountCategory == "4")) {
                             trialBalanceContent += '<tr><td>' + (i + 1) + '</td><td>' + data[i].Key.GlAccountName + '</td><td></td><td>' + data[i].Value + '</td></tr>';
                             creditSum += data[i].Value;
                         }
                     }
                     debitSum = debitSum.toFixed(2);
                     creditSum = creditSum.toFixed(2);
                     $('#trialBalance').append(trialBalanceContent);
                     $('#trialBalance').append('<tr><td></td><td></td><td><b>' + debitSum + '</b></td><td><b>' + creditSum + '</b></td></tr>');
                     
                 },
                 error: function (xhr, status, error) {
                     $("#trialBalance").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                 }
             });
         });
         
     </script>
</asp:Content>
