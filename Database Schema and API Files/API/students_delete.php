<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

$id = $data->id;

$sql = "DELETE FROM students WHERE ID=$id";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Success"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>
