<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="GenViewBalSheet.aspx.cs" Inherits="CBAPractice.FinancialRepMgt.GenViewBalSheet" %>
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
                          <th></th>
                        <th></th>
                        <th></th>
                        <th>(₦)</th>
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
                     $('#title').html("Balance Sheet as at " + $('#date').val());
                     var trialBalanceContentforAssets = '';
                     var trialBalanceContentforLiabilities = '';
                     var trialBalanceContentforCapital = '';
                     $('#trialBalance tbody').remove();
                     var AssetSum = 0;
                     var LiabilitySum = 0;
                     var CapitalSum = 0;
                     var TotalAssets = 0;
                     var TotalLiabilitiesAndCapital = 0;
                     var IncomeSum = 0;
                     var ExpenseSum = 0;
                     var ProfitOrLoss = 0;
                     var LossMadePositive = 0;
                     
                     for (i = 0; i < data.length; i++) {

                         if (data[i].Key.GlCategory.MainAccountCategory == "4") {
                             IncomeSum += data[i].Value;
                         }
                     }
                     IncomeSum = IncomeSum.toFixed(2);
                     for (i = 0; i < data.length; i++) {
                         if (data[i].Key.GlCategory.MainAccountCategory == "5") {
                             ExpenseSum += data[i].Value;
                         }
                     }
                     ExpenseSum = ExpenseSum.toFixed(2);
                     ProfitOrLoss = IncomeSum - ExpenseSum;
                     ProfitOrLoss = ProfitOrLoss.toFixed(2);
                     LossMadePositive = -ProfitOrLoss; 
                     LossMadePositive = LossMadePositive.toFixed(2);
                     $('#trialBalance').append('<tr><td><b>' + "Assets" + '</b></td><td><b></b></td><td><b></b></td></tr>');
                     for (i = 0; i < data.length; i++) {

                         if (data[i].Key.GlCategory.MainAccountCategory == "1") {
                             trialBalanceContentforAssets += '<tr><td>' + data[i].Key.GlAccountName + '</td><td></td><td></td><td>' + data[i].Value + '</td></tr>';
                             AssetSum += data[i].Value;
                         }
                     }
                     $('#trialBalance').append(trialBalanceContentforAssets);
                     if (ProfitOrLoss < 0) {
                         TotalAssets = parseFloat(AssetSum) + parseFloat(LossMadePositive);
                         $('#trialBalance').append('<tr><td>' + "Loss" + '</td><td><b></b></td><td><b></b></td><td>' + LossMadePositive + '</td></tr>');
                         $('#trialBalance').append('<tr><td><b>' + "Total Assets" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + TotalAssets + '</b></td></tr>');
                     }
                     else if (ProfitOrLoss >= 0) {
                         $('#trialBalance').append('<tr><td>' + "Loss" + '</td><td><b></b></td><td><b></b></td><td>' + 0 + '</td></tr>');
                         $('#trialBalance').append('<tr><td><b>' + "Total Assets" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + AssetSum + '</b></td></tr>');
                     }
                     
                     $('#trialBalance').append('<tr><td><b>' + "Liabilities" + '</b></td><td><b></b></td><td><b></b></td></tr>');

                     for (i = 0; i < data.length; i++) {
                         if (data[i].Key.GlCategory.MainAccountCategory == "2") {
                             trialBalanceContentforLiabilities += '<tr><td>' + data[i].Key.GlAccountName + '</td><td></td><td></td><td>' + data[i].Value + '</td></tr>';
                             LiabilitySum += data[i].Value;
                         }
                     }
                     $('#trialBalance').append(trialBalanceContentforLiabilities);
                     $('#trialBalance').append('<tr><td><b>' + "Total Liabilities" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + LiabilitySum + '</b></td></tr>');
                     $('#trialBalance').append('<tr><td><b>' + "Equities(Capital)" + '</b></td><td><b></b></td><td><b></b></td></tr>');
                     
                     for (i = 0; i < data.length; i++) {
                         if (data[i].Key.GlCategory.MainAccountCategory == "3") {
                             trialBalanceContentforCapital += '<tr><td>' + data[i].Key.GlAccountName + '</td><td></td><td></td><td>' + data[i].Value + '</td></tr>';
                             CapitalSum += data[i].Value;
                         }
                     }
                     $('#trialBalance').append(trialBalanceContentforCapital);
                     $('#trialBalance').append('<tr><td><b>' + "Total Equities" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + CapitalSum + '</b></td></tr>');
                     if (ProfitOrLoss < 0) {
                         TotalLiabilitiesAndCapital = parseFloat(LiabilitySum) + parseFloat(CapitalSum);
                         $('#trialBalance').append('<tr><td>' + "Profit" + '</td><td><b></b></td><td><b></b></td><td>' + 0 + '</td></tr>');
                         $('#trialBalance').append('<tr><td><b>' + "Total Liabilities And Equities" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + TotalLiabilitiesAndCapital + '</b></td></tr>');
                     }
                     else if (ProfitOrLoss > 0) {
                         TotalLiabilitiesAndCapital = parseFloat(LiabilitySum) + parseFloat(CapitalSum) + parseFloat(ProfitOrLoss);
                         $('#trialBalance').append('<tr><td>' + "Profit" + '</td><td><b></b></td><td><b></b></td><td>' + ProfitOrLoss + '</td></tr>');
                         $('#trialBalance').append('<tr><td><b>' + "Total Liabilities And Equities" + '</b></td><td><b></b></td><td><b></b></td><td><b>' + TotalLiabilitiesAndCapital + '</b></td></tr>');
                     }
                     

                 },
                 error: function (xhr, status, error) {
                     $("#trialBalance").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                 }
             });
         });

     </script>
</asp:Content>



