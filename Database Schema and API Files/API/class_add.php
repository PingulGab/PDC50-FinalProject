<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

$image = $data->image;
$name = $data->name;
$acronym = $data->acronym;
$days = $data->days;
$startTime = $data->startTime;
$endTime = $data->endTime;

$sql = "INSERT INTO classes (Image, Name, Acronym, Days, StartTime, EndTime) VALUES ('$image', '$name', '$acronym', '$days', '$startTime', '$endTime')";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Success"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>