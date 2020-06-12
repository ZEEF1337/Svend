<?php
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Credentials: true");
header("Access-Control-Max-Age: 1000");
header("Access-Control-Allow-Headers: X-Requested-With, Content-Type, Origin, Cache-Control, Pragma, Authorization, Accept, Accept-Encoding");
header("Access-Control-Allow-Methods: GET");
header("Content-Type: application/json; charset=UTF-8");

include_once ($_SERVER['DOCUMENT_ROOT']."/SvendAPI/functions/propFunctions.php");


if(!isset($_GET['token'])){
    $out['result'] = 0;
    $out['message'] = "Mangler parameter";
    $json = json_encode($out);
    print_r($json);
    return;
}

$givenToken = $_GET['token'];

$database = new Database();
$db = $database->getConnection();

$UserID = getUserIDFromToken($givenToken);

$query = "SELECT Fornavn, Efternavn, Email FROM users WHERE ID = $UserID;";
$stmt = $db->prepare($query);

try{
    $stmt->execute();
    $num = $stmt->rowCount();
    if($num > 0){
        $row = $stmt->fetch(PDO::FETCH_ASSOC);
        extract($row);
        $out['result'] = 1;
        $out['Fornavn'] = $Fornavn;
        $out['Efternavn'] = $Efternavn;
        $out['Email'] = $Email;
    }else{
        $out['result'] = 0;
        $out['message'] = "Ingen resultater fundet";
    }
} catch(PDOException $e){
    $out['result'] = 0;
    $out['message'] = $e->getMessage();
}
$json = json_encode($out);
print_r($json);
return;
?>