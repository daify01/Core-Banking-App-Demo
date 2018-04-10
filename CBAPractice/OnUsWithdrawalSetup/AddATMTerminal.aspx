<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddATMTerminal.aspx.cs" Inherits="CBAPractice.OnUsWithdrawalSetup.AddATMTerminal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <!-- Content Header (Page header) -->
        <section class="content-header">
          <h1>
            On Us Withdrawal Setup
            <small>Add/Edit ATM Terminal</small>
          </h1>
        </section>

        <!-- Main content -->
        <section class="content">
          <div class="row">
            <!-- left column -->
            <div class="col-md-6" style="width: 100%">
              <!-- general form elements -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Enter ATM Terminal Details</h3>
                </div><!-- /.box-header -->
                 
                   <%--runat="server"--%>
                <!-- form start -->
                <form role="form" runat="server" ClientIDMode="Static" id="form1" >
                  <div class="box-body">
                      <div class="col-md-6">
                      <input type="hidden" runat="server" clientidmode="Static" id="TextBoxId"/>
                    <div class="form-group">
                      <label for="TextBoxNameTerminalName">Terminal Name</label>
                        <input type="text" clientidmode="Static" runat="server" class="form-control" id="TextBoxNameTerminalName"  pattern="^[a-zA-Z_]+( [a-zA-Z_]+)*$" title="All Branchnames" maxlength="20" required="required" placeholder="Enter Name"/>
                    </div>
                    <div class="form-group">
                      <label for="TextBoxNameTerminalID">Terminal ID</label>
                        <input type="text" runat="server" clientidmode="Static" class="form-control" id="TextBoxTextBoxNameTerminalID" title="Branch RC Numbers" pattern="[0-9]*" maxlength="20" required="required" placeholder="RcNumber"/>
                    </div>
                    <div class="form-group">
                      <label for="TextBoxNameLocation">Location</label>
                        <textarea runat="server" clientidmode="Static" class="form-control" id="TextBoxNameLocation" title="Branch RC Numbers" pattern="" maxlength="300" required="required" placeholder="Address" ></textarea>
                    </div>
                      

                  <div class="box-footer">
                    <%--<button id="searchsubmit" type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" class="btn btn-flat btn-primary">Add</button>
                    <button type="submit" class="btn btn-primary">Edit</button>--%>
                      <input type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" id="searchsubmit" class="btn btn-flat btn-primary pull-right" name="Add"/>
                  </div>
                          </div>
                      </div>
                </form>
              </div><!-- /.box -->
                
                 </div><!--/.col (right) -->
          </div>   <!-- /.row -->
        </section><!-- /.content -->
</asp:Content>
