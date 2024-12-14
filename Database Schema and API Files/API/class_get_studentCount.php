<?php
header("Access-Control-Allow-Origin: *");
header("Content-Type: application/json; charset=UTF-8");

include 'db.php'; // Include your database connection file

// Get the classId from the request
$classId = isset($_GET['classId']) ? intval($_GET['classId']) : 0;

if ($classId > 0) {
    // SQL query to count the number of students in the given class
    $sql = "SELECT COUNT(*) AS studentCount FROM students WHERE class_id = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("i", $classId);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($row = $result->fetch_assoc()) {
        echo json_encode(["studentCount" => $row['studentCount']]);
    } else {
        echo json_encode(["studentCount" => 0]);
    }

    $stmt->close();
} else {
    echo json_encode(["error" => "Invalid classId"]);
}

$conn->close();
?>
