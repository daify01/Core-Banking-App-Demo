<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="GenViewPL.aspx.cs" Inherits="CBAPractice.FinancialRepMgt.GenViewPL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Financial Report Management
            <small>P&L</small>
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
                        <th>Income/Expense(₦)</th>
                        <th>Profit(₦)</th>
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
                 data: { date: $('#date').val() },

                 success: function (data) {
                     debugger;
                     $('#title').html("Profit And Loss Statement as at " + $('#date').val());
                     var trialBalanceContentforAssets = '';
                     var trialBalanceContentforExpense = '';
                     $('#trialBalance tbody').remove();
                     var IncomeSum = 0;
                     var ExpenseSum = 0;
                     var ProfitOrLoss = 0;
                     var LossMadePositive = 0;
                     $('#trialBalance').append('<tr><td><b>' + "Income" + '</b></td><td><b></b></td><td><b></b></td></tr>');
                     for (i = 0; i < data.length; i++) {

                         if (data[i].Key.GlCategory.MainAccountCategory == "4") {
                             trialBalanceContentforAssets += '<tr><td>' + data[i].Key.GlAccountName + '</td><td>' + data[i].Value + '</td><td></td></tr>';
                             IncomeSum += data[i].Value;
                         }
                     }
                     $('#trialBalance').append(trialBalanceContentforAssets);
                     IncomeSum = IncomeSum.toFixed(2);
                     $('#trialBalance').append('<tr><td><b>' + "Total Income" + '</b></td><td><b></b></td><td><b>' + IncomeSum + '</b></td></tr>');
                     $('#trialBalance').append('<tr><td><b>' + "Expense" + '</b></td><td><b></b></td><td><b></b></td></tr>');

                     for (i = 0; i < data.length; i++) {
                         if (data[i].Key.GlCategory.MainAccountCategory == "5") {
                             trialBalanceContentforExpense += '<tr><td>' + data[i].Key.GlAccountName + '</td><td>' + data[i].Value + '</td><td></td></tr>';
                             ExpenseSum += data[i].Value;
                         }
                     }
                     $('#trialBalance').append(trialBalanceContentforExpense);
                     ExpenseSum = ExpenseSum.toFixed(2);
                     $('#trialBalance').append('<tr><td><b>' + "Total Expense" + '</b></td><td><b></b></td><td><b>' + ExpenseSum + '</b></td></tr>');
                     ProfitOrLoss = IncomeSum - ExpenseSum;
                     ProfitOrLoss = ProfitOrLoss.toFixed(2);
                     LossMadePositive = -ProfitOrLoss;
                     LossMadePositive = LossMadePositive.toFixed(2);
                     if (ProfitOrLoss > 0) {
                         $('#trialBalance').append('<tr><td><b>' + "PROFIT" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + ProfitOrLoss + '</b></td></tr>');
                     }
                     if (ProfitOrLoss < 0) {
                         $('#trialBalance').append('<tr><td><b>' + "LOSS" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + LossMadePositive + '</b></td></tr>');
                     }

                 },
                 error: function (xhr, status, error) {
                     $("#trialBalance").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                 }
             });
         });

     </script>
</asp:Content>


