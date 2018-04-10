<%@ Page Title="" Language="C#" MasterPageFile="~/CBA.Master" AutoEventWireup="true" CodeBehind="AddNewCustomer.aspx.cs" Inherits="CBAPractice.ManageCustomers.AddNewCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Customer Management
            <small>Add/Edit Customer</small>
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

                    <!-- form start -->
                    <form id="form1" runat="server">
                        <div class="box-body">
                            <div class="col-md-6">
                             <input type="hidden" runat="server" id="TextBoxId"/>
                            <div class="form-group">
                                <label for="TextBoxNameFName">FirstName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameFName" title="Firstnames" pattern="[A-Za-z]*" maxlength="20" required="" placeholder="Enter Name"/>
                                <%--<asp:Label ID="firstnamelabel" runat="server" Font-Bold="true">FirstName</asp:Label>
                                <asp:TextBox ID="FirstNameTextbox" class="form-control" placeholder="FirstName" runat="server" />
                                <asp:RequiredFieldValidator ID="FirstNameReq"
                                    runat="server"
                                    ControlToValidate="FirstNameTextbox"
                                    ErrorMessage="Firstname is required!" ForeColor="red" />--%>
                                <%--<input type="text" class="form-control" id="BranchName" placeholder="Enter Name">--%>
                            </div>
                             <div class="form-group">
                                 <label for="TextBoxNameLName">LastName</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameLName" title="LastName" pattern="[A-Za-z]*" maxlength="20" required="" placeholder="Enter Name"/>
                                <%--<asp:Label ID="lastnamelabel" runat="server" Font-Bold="true">LastName</asp:Label>
                                <asp:TextBox ID="LastNameTextBox" class="form-control" placeholder="LastName" runat="server" />
                                <asp:RequiredFieldValidator ID="LastValidator"
                                    runat="server"
                                    ControlToValidate="LastNameTextbox"
                                    ErrorMessage="Lastname is required!" ForeColor="red" />--%>
                                <%--<input type="text" class="form-control" id="BranchName" placeholder="Enter Name">--%>
                            </div>
                            <div class="form-group">
                                <label for="TextBoxNameONames">OtherNames</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameONames" title="OtherNames" maxlength="20" placeholder="Enter Name"/>
                                <%--<asp:Label ID="OtherNames" runat="server" Font-Bold="true">OtherNames</asp:Label>
                                <asp:TextBox ID="OtherNamesTextbox" class="form-control" placeholder="OtherNames" runat="server" />--%>
                                <%--<label for="Address">Address</label>
                                <input type="text" class="form-control" id="Address" placeholder="Enter Address">--%>
                            </div>
                            <div class="form-group">
                      <label for="TextBoxNameAddress">Address</label>
                        <textarea runat="server" class="form-control" id="TextBoxNameAddress" title="Enter Address Here" pattern="" maxlength="300" required="" placeholder="Address" ></textarea>
                    </div>
                                </div>
                            
                            <div class="col-md-6">
                             <div class="form-group">
                                <label for="TextBoxNamePhone">PhoneNumber</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNamePhone" title="11 digit phone number, no spacing" pattern="[0-9][0-9]{10,10}" maxlength="15" required="" placeholder="Enter Phone"/>
                               <%-- <asp:Label ID="PhoneNumber" runat="server" Font-Bold="true">Phone Number</asp:Label>
                                <asp:TextBox ID="PhoneNumberTextBox" class="form-control" placeholder="PhoneNumber" runat="server" />
                                <asp:RequiredFieldValidator ID="PhoneNumberreq"
                                    runat="server"
                                    ControlToValidate="PhoneNumberTextBox"
                                    ErrorMessage="Phone Number is required!" ForeColor="red" Display="Dynamic" />
                                
                                <asp:RegularExpressionValidator ID="PhoneNumberValidator"
                                    runat="server" ControlToValidate="PhoneNumberTextBox"
                                    ValidationExpression="^(\d){11}$"
                                    ErrorMessage="You must enter a valid phone number!" ForeColor="red" />--%>
                            </div>
                            <div class="form-group">
                                <label for="TextBoxNameEmail">Email</label>
                        <input type="text" runat="server" class="form-control" id="TextBoxNameEmail" title="Enter proper email format" pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$" maxlength="80" required="" placeholder="Enter Email"/>
                               <%-- <asp:Label ID="Email" runat="server" Font-Bold="true">Email</asp:Label>
                                <asp:TextBox ID="EmailTextbox" class="form-control" placeholder="Email" runat="server" />
                                <asp:RequiredFieldValidator ID="EmailReq"
                                    runat="server"
                                    ControlToValidate="EmailTextbox"
                                    ErrorMessage="Email is required!" ForeColor="red" Display="Dynamic"  />

                                <asp:RegularExpressionValidator ID="emailValidator"
                                    runat="server" ControlToValidate="EmailTextBox"
                                    ValidationExpression="^\S+@\S+\.\S+$"
                                    ErrorMessage="You must enter a valid email address!" ForeColor="red"/>--%>
                                <%--<label for="BranchName">BranchName</label>
                                <input type="text" class="form-control" id="Text2" placeholder="Enter Name">--%>
                            </div>
                            
                            <div class="form-group">
                                <asp:Label ID="Gender" runat="server" Font-Bold="true">Gender</asp:Label>
                                <asp:DropDownList ID="DropDownGender" class="form-group" runat="server" ClientIDMode="Static" required="required" AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem/>
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="RoleTextBox" class="form-control" placeholder="Role" runat="server" />
                                <asp:RequiredFieldValidator ID="RoleFieldValidator"
                                    runat="server"
                                    ControlToValidate="RoleTextBox"
                                    ErrorMessage="Role is required!" ForeColor="red" />--%>
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
    <script type="text/javascript">
        $(function () {
            $('#DropDownGender').select2({ placeholder: "Please Select Gender" });

        });
    </script>

</asp:Content>
