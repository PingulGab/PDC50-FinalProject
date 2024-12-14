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
$sql = "SELECT * FROM students WHERE ClassID = $id";
$result = $conn->query($sql);

$users = [];
while ($row = $result->fetch_assoc()) {
    $users[] = $row;
}

echo json_encode($users);

$conn->close();
?>
