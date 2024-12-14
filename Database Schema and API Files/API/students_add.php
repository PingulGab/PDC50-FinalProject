<?php
include 'db.php';

$data = json_decode(file_get_contents("php://input"));

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

$sql = "INSERT INTO students (Image, Name, Gender, StudentID, ContactNumber, ClassID, Birthdate, ElementaryEducation, SecondaryEducation, TertiaryEducation)
        VALUES ('$image', '$name', '$gender', '$studentID', '$contactNumber', $classID, '$birthdate', '$elementaryEducation', '$secondaryEducation', '$tertiaryEducation')";

if ($conn->query($sql) === TRUE) {
    echo json_encode(["message" => "Success"]);
} else {
    echo json_encode(["message" => "Error: " . $conn->error]);
}

$conn->close();
?>