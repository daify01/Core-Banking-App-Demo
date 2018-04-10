<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="RunEOD.aspx.cs" Inherits="CBAPractice.EODProcess.RunEOD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Savings Account Configuration
            <small>Edit Account Configuration</small>
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
                  <h3 class="box-title">Configure Savings Account</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            
                            <div class="form-group">
                                 <button id="Button2" type="submit" runat="server" OnServerClick="closesubmit_OnServerClick" class="btn btn-primary">Close Business</button>
                            </div>
                            
                            <div class="form-group">
                               <button id="Button3" type="submit" runat="server" OnServerClick="opensubmit_OnServerClick" class="btn btn-primary">Open Business</button>
                            </div>
                            
                           
                </div>
                <!-- /.box -->
                        </form>

            </div>
            <!--/.col (right) -->
        </div>
        <!-- /.row -->
              </div>
    </section>
    <!-- /.content -->







</asp:Content>

