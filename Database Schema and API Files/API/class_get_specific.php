<?php
// Allow access from any origin and specify JSON content type
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

// Include the database connection
include 'db.php';

// Decode the incoming JSON payload
$data = json_decode(file_get_contents("php://input"));

$id = $_GET['id'];

// Prepare and execute the query
$sql = "SELECT * FROM classes WHERE ID = $id";
$result = $conn->query($sql);

// Initialize an array to hold the response
$response = [];

if ($result && $result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $response[] = $row;
    }
    echo json_encode($response);
} else {
    echo json_encode(["error" => "Class not found"]);
}

// Close the database connection
$conn->close();
?>
