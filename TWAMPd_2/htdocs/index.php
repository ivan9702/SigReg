<?php
$ctry = explode(',', $_SERVER['HTTP_ACCEPT_LANGUAGE']);
$lang = getlang($ctry);

switch ($lang) {
  case 'zh':
  if ( substr($ctry[0], -2) == 'cn' ) {
    $url = 'twconfig.php?lang=cn';
  } else {
    $url = 'twconfig.php?lang=tw';
  }
  break;

  default:
  $url = 'twconfig.php?lang='. $lang;
  break;
}

header("Location: {$url}");
exit;


function getlang($ctry) { return substr(strtolower($ctry[0]), 0, 2);}

?><html><body><h1>It works!</h1></body></html>
