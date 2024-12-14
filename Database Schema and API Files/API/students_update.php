<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

$id = $data->id;
$image = $data->image;
$name = $data->name;
$gender = $data->gender;
$studentID = $data->studentID;
$contactNumber = $data->contactNumber;
$classID = $data->classID;
$birthdate = $data->birthdate;
$elementaryEducation = $data->elementaryEducation;
$secondaryEducation = $data->secondaryEducation;
$tertiaryEducation = $data->tertiaryEducation;

$sql = "UPDATE students SET Image='$image', Name='$name', Gender='$gender', StudentID='$studentID', ContactNumber='$contactNumber', ClassID=$classID, Birthdate='$birthdate', ElementaryEducation='$elementaryEducation', SecondaryEducation='$secondaryEducation', TertiaryEducation='$tertiaryEducation' WHERE ID=$id";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Success"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>