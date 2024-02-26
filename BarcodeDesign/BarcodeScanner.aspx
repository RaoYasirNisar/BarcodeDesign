<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarcodeScanner.aspx.cs" Inherits="BarcodeDesign.BarcodeScanner" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Barcode Scanner</title>
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/quagga/dist/quagga.min.js"></script>
    <script type="text/javascript">
        // Function to initiate barcode scanning
        function scanBarcode() {
            Quagga.decodeSingle({
                inputStream: {
                    name: "Live",
                    type: "LiveStream",
                    target: document.querySelector('#barcodeScanner'), // DOM element to attach the camera stream
                    constraints: {
                        width:200,
                        height: 200,
                        facingMode: "environment" // Use the device's rear camera (if available)
                    }
                },
                decoder: {
                    readers: ["ean_reader"] // Specify the barcode format you want to scan
                },
                locate: true,
                singleResult: true, // Only decode one barcode and stop
                numOfWorkers: 4, // Increase the number of decoding workers
                frequency: 10, // Increase the frequency of decoding attempts (in Hz)
            }, function (result) {
                if (result && result.codeResult) {
                    var barcode = result.codeResult.code;
                    console.log("Barcode detected and read: " + barcode);
                    // Send the barcode data to the server for processing
                    sendBarcodeToServer(barcode);
                } else {
                    console.log("No barcode detected");
                    // Optionally provide feedback to the user if no barcode is detected
                }
            });
        }

        //    Quagga.onDetected(function (result) {
        //        var barcode = result.codeResult.code;
        //        console.log("Barcode detected and read: " + barcode);
        //        // You can now send this barcode to your server for further processing/validation
        //        // For example, using AJAX to send it to a server-side endpoint
        //        sendBarcodeToServer(barcode);
        //        Quagga.stop();
        //    });
        //}

        // Function to send the scanned barcode to the server for processing/validation
        function sendBarcodeToServer(barcode) {
            // You can use AJAX to send the barcode to a server-side endpoint
            $.ajax({
                type: "POST",
                url: "BarcodeScanner.aspx/ValidateBarcode",
                data: JSON.stringify({ barcode: barcode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Handle response from the server
                    console.log("Validation result:", response);
                    // Update UI or perform other actions based on the validation result
                    document.getElementById("validationResult").innerHTML = response.d.exists ? "Asset exists: " + response.d.assetInfo : "Asset does not exist.";
                },
                error: function () {
                    console.error("An error occurred while validating the barcode.");
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Barcode Scanner</h1>
            <div id="barcodeScanner" style="width: 100%; height: 300px;"></div>
            <asp:Button ID="btnScan" runat="server" OnClientClick="scanBarcode()" Text="Scan"></asp:Button>
            <div id="validationResult"></div>
        </div>
    </form>
</body>
</html>
