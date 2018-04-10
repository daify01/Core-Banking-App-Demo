<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="EditCustomerAcct.aspx.cs" Inherits="CBAPractice.ManageCustomerAcct.EditCustomerAcct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
   <!-- Main content -->
     <section class="content-header">
        <h1>Customer Account Management
           
            <small>Add/Edit Customer Accounts</small>
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
                  <h3 class="box-title">Enter Customer Details</h3>
                </div><!-- /.box-header -->
                    <!-- /.box-header -->
                  
                   <%--<div class="well well-lg" style="margin: 0.8em;">
                <div class="row">
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchname">Name</label>
                              <input ClientIDMode="Static" id="searchname" class="form-control" name="name" type="text" runat="server"/>
                          </div>
                      </div>
                      <div class="col-md-6">
                          <div class="form-group">
                              <label for="searchaddress">Address</label>
                              <input id="searchaddress" name="code" ClientIDMode="Static" class="form-control" type="text" runat="server"/>
                          </div>
                          <button type="button" runat="server" ClientIDMode="Static" id="searchsubmit" class="btn btn-flat btn-primary pull-right" OnServerClick="searchsubmit_OnServerClick">Submit</button>
                      </div> 
                  </div>
                </div>--%>

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNameAcctName">AccountName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameAcctName" title="Account Name" pattern="^([a-zA-Z_][a-zA-Z_ ]*[a-zA-Z_]$" maxlength="120" required="" placeholder="Surname FirstName MiddleName"/>
                               
                            </div>
                             
                            <div class="form-group">
                                <asp:Label ID="BranchName" runat="server" Font-Bold="true">Branch</asp:Label>
                                <asp:DropDownList ID="DropDownBranchName" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control" DataTextField="BranchName" DataValueField="Id">
                                    <asp:ListItem/>
                                </asp:DropDownList>
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="AccountType" runat="server" Font-Bold="true">AccountType</asp:Label>
                                <asp:DropDownList ID="DropDownAccountType" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem/>
                                </asp:DropDownList>
                            </div>
                            
                           
                        
                       

                        <div class="box-footer">
                            <input type="submit" runat="server" OnServerClick="searchsubmit_OnServerClick" id="searchsubmit" class="btn btn-flat btn-primary pull-right" name="Add"/>
                           
                        </div>
                                </div>
                            </div>
                        
                    </form>
                </div>
                <!-- /.box -->

            </div>
            <!--/.col (right) -->
        </div>
        <!-- /.row -->
    </section>
    <!-- /.content -->
   
    <div class="modal fade" id="ViewCustomerDetailsModal" tabindex="-1" role="dialog" aria-labelledby="ViewCustomerDetailsModal" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Customer Details</h4>
            </div>
            <div class="modal-body" id="detailsContent" style="overflow-y:scroll; max-height:450px">
                
                
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
            var table = $("#viewcustomers").DataTable({
                "serverSide": true,
                "ajax": {
                    "url": '/api/Query/CustomerAccounts', //url to fetch data from
                    "type": 'POST',
                    data: function (d) {
                        d.searchName = $('#searchname').val();
                        d.searchAddress = $('#searchaddress').val();
                    }
                },
                "searching": false,
                columns: [
                    { data: 'ID' },
                { data: 'FIRSTName' },
                    { data: 'LASTName' },
                    { data: 'OTHERNames' },
                    { data: 'EMail' },
                    { data: 'GENder' },
                    { data: 'AddAccounts' }
                ]
            });

            table.on('order.dt search.dt', function () {
                table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            $("#searchsubmit").click(function () {
                table.search('').draw();
            });
        });

        
            
        function Redirect(id) {
            $('#ViewCustomerDetailsModal').modal('show');
            $.ajax({
                url: '/api/Query/CustomerDetails?id=' + id,
                method: 'POST',
                success: function (result) {
                    //$("#detailsContent").html(result);
                    //$('#detailFName').html(result.FirstName);
                    //$('#detailLName').html(result.LastName);
                    //$('#detailOName').html(result.OtherNames);
                    //$('#detailEmail').html(result.Email);
                    //$('#detailGender').html(result.GenderString);
                    //$('#detailCustomerID').html(result.Id);
                    //$('#detailPhoneNumber').html(result.PhoneNumber);
                    //$('#detailAddress').html(result.Address);
                    $("#Edit").attr("data-id", id);
                },
                error: function (xhr, status, error) {
                    $("#detailsContent").html("<h2><i class='fa fa-warning'></i> Couldn't fetch details, pls try again</h2>");
                }
            });
            }

        $("#Edit").click(function() {
            location = '../ManageCustomerAcct/EditCustomerAcct.aspx?id=' + $(this).attr("data-id");
        });
        
        $(function () {
            $('#DropDownBranchName').select2({ placeholder: "Please Select A Branch Name" }),
            $('#DropDownAccountType').select2({ placeholder: "Please Select An Account Type" });
        });
    </script>






</asp:Content>
