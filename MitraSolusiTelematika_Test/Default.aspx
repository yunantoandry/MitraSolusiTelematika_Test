<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MitraSolusiTelematika_Test._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="Scripts/Custom.js"></script>
    <script src="Scripts/WebForms/Jquery-1.12.4.js"></script>
    <script src="Scripts/WebForms/jquery.dataTables.min.js"></script>
    <link href="Scripts/WebForms/Pagination.css" rel="stylesheet" />
    <script src="Scripts/dataTables.bootstrap.js"></script>
    <link href="Scripts/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        .Hide {
            display: none;
        }
    </style>

    <div class="box-header">
        <div class="row">
            <div class="form-group" style="display: none;">
                <asp:TextBox ID="txtiD" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <label>No Kode Pos<span style="color: red">*</span></label>
                <asp:TextBox ID="txtKodePos" runat="server" class="form-control" placeholder="Please Input Kode Pos"></asp:TextBox>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <label>Kelurahan<span style="color: red">*</span></label>
                <asp:TextBox ID="txtKelurahan" runat="server" class="form-control" placeholder="Please Input Kelurahan"></asp:TextBox>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <label>Kecamatan<span style="color: red">*</span></label>
                <asp:TextBox ID="txtKecamatan" runat="server" class="form-control" placeholder="Please Input Kecamatan"></asp:TextBox>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <label>Jenis<span style="color: red">*</span></label>
                <asp:TextBox ID="txtJenis" runat="server" class="form-control" placeholder="Please Input Jenis"></asp:TextBox>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <label>Kabupaten<span style="color: red">*</span></label>
                <asp:TextBox ID="txtKabupaten" runat="server" class="form-control" placeholder="Please Input Kabupaten"></asp:TextBox>
            </div>
        </div>
        <br>
        <div class="row">
            <div class="col-md-12">
                <label>Propinsi<span style="color: red">*</span></label>
                <asp:TextBox ID="txtPropinsi" runat="server" class="form-control" placeholder="Please Input Propinsi"></asp:TextBox>
            </div>
        </div>
        <br>

        <br>
        <div class="row">
            <div class="col-md-12">
                <div class="col-xs-6">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" class="btn btn-block btn-primary" Text="Save" />
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" class="btn btn-block btn-primary" Text="Update" />
                </div>
                <div class="col-xs-6">
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" class="btn btn-block btn-default" Text="Cancel" />
                </div>
            </div>
        </div>
    </div>

    <div class="col-md-8">
        <div class="box">
            <div class="box-body no-padding">
                <asp:Panel ID="PanelDetail" runat="server">
                    <div id="divData" runat="server" visible="true">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <b>List Of Kode Pos</b>
                            </div>
                            <div class="panel-body">
                                <div class="table-responsive">
                                    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand" CssClass="table table-bordered bs-table"
                                        Width="100%" OnRowEditing="gv_RowEditing" OnRowDeleting="gv_RowDeleting" OnRowDataBound="gv_RowDataBound">
                                        <Columns>

                                            <asp:BoundField DataField="ID" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="hide" />
                                            <asp:BoundField DataField="NO_KODE_POS" HeaderText="No Kode Pos" />
                                            <asp:BoundField DataField="KELURAHAN" HeaderText="Kelurahan" />
                                            <asp:BoundField DataField="KECAMATAN" HeaderText="Kelurahan" />
                                            <asp:BoundField DataField="JENIS" HeaderText="Jenis" />
                                            <asp:BoundField DataField="KABUPATEN" HeaderText="Kabupaten" />
                                            <asp:BoundField DataField="PROPINSI" HeaderText="Propinsi" />
                                            <asp:ButtonField runat="server" Text="Edit" CommandName="Edt" ControlStyle-CssClass="btn btn-block btn-primary" ButtonType="Button" />
                                            <asp:ButtonField runat="server" Text="Delete" CommandName="Del" ControlStyle-CssClass="btn btn-block btn-danger" ButtonType="Button" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="text-left">
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>
