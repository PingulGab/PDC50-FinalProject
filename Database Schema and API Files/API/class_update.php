<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

$id = $data->id;
$image = $data->image;
$name = $data->name;
$acronym = $data->acronym;
$days = $data->days;
$startTime = $data->startTime;
$endTime = $data->endTime;

$sql = "UPDATE classes SET Image='$image', Name='$name', Acronym='$acronym', Days='$days', StartTime='$startTime', EndTime='$endTime' WHERE ID=$id";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Success"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>