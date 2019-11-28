TWAMP
Tiny Windows Apache MySQL PHP - Portable Web Server


TWAMP 麻瓜架站包 v7.03 好一朵美麗的茉莉花版
( 在政變都可以群眾外包的時代，茉莉花除了在集權國家開花也可以用來改善民主之惡 )


    * TWAMP (Tiny Windows Apache MySQL PHP)
    * Version: 7.03a from 25th Feb 2011
    * Author: Yelban Hsu
    * orz99.com - TWAMP
    * Support and developer's blog

Includes

    * Apache 2.2.17
    * MySQL 5.1.49-community
    * PHP 5.2.17
    * phpMyAdmin 3.3.9.2
    * perl 5.12.3

» About

TWAMP is one of AMP (Apache, MySQL, PHP) software bundles for Windows computer platforms; these solution stacks are used to run dynamic Web sites or servers, and interpreters for scripts written in the PHP and Perl programming languages.

TWAMP can run from media (USB drives, flash disks, etc) . It can be activated without any installation, the package download size is under 9MB.

TWAMP is commonly used to test web applications on Windows, and can also be set up on removable media for a portable web server.

» Downloads

    * http://orz99.com/twamp

» Features

    * Need no installation and run anywhere
    * triple op code accelerator included
    * Blitz, the fastest PHP template engine included
    * Most popular 3rd party PHP extension included
    * SSL READY, Self-signed certificate included
    * Perl CGI READY http://localhost/cgi-bin/printenv.pl
    * fully pass of yii v1.0.6.r1102 test
    * fully pass of magento v1.3.2.1 test
    * fully pass of vtigercrm v5.0.4 test

» CHANGE LOGS







Changes in 7.03

    * add php_pdo_sqlsrv_52_ts_vc6
    * add Drupal-7.0
    * update to PHP 5.2.17
    * update to MySQL 5.1.49-community
    * update to phpMyAdmin 3.3.9.2
    * update to Perl 5.12.3
    * update Zend Optimizer 3.3.0
    * update to Drupal-6.20
    
Changes in 2.2

    * update to PHP 5.2.14
    * Add eAccelerator v1_0_svn425
    * update to MySQL 5.1.41-community
    * update to phpMyAdmin 3.2.4.0
    
Changes in 2.1

    * New PHP FreeTDS included
    * New PHP MongoDB support included
    * New PHP mailparse support included
    * New PHP uploadprogress support included
    * Remove PHP blitz
    * Remove PHP eAccelerator-0.9.6-rc1
    * Remove PHP blitz

Changes in 2.0-6.x

    * New TWAMP Web Configurator included
    * New PHP Oauth included
    * update to MySQL 5.1.41-community
    * update to phpMyAdmin 3.2.4.0
    * update to blitz 5.2.12.12
    * update to xcache 5.2.10.10

Changes in 1.3.6.1

    * update to MySQL 5.1.40-community
    * update to phpMyAdmin 3.2.3.0
    * update to eAccelerator-0.9.6-rc1

Changes in 1.3.5

    * New DBG included
    * update to PHP 5.2.11 (eAccelerator)
    * update to MySQL 5.1.39-community
    * update to phpMyAdmin 3.2.2.0

Changes in 1.3.4

    * update startup path arguments, fixed running problem where startup path include space character
    * update to Apache 2.2.13
    * update to MySQL 5.1.38-community
    * update to phpMyAdmin 3.2.1.0

Changes in 1.3.3

    * Append Japanese and Chinese Simplified Languages for Drupal (TWAMPd only)

Changes in 1.3.2

    * PEAR path fixed
    * update to Drupal 6.13 (TWAMPd only)

Changes in 1.3.1

    * Zend Optimizer path fixed
    * update to PHP 5.2.10

Changes in 1.3.0-pear

    * New PEAR included
    * New blitz template engine included
    * New memcache included
    * update to MySQL 5.1.36-community
    * update to phpMyAdmin 3.2.0.1
    * update to eAccelerator-0.9.5.3

Changes in 1.2.3-12

    * update to Drupal 6.12 (TWAMPd)

Changes in 1.2.3

    * PHP xDebug is disabled by default (improve performance)
    * Drupal 6.11 Chinese Traditional language enhanced (TWAMPd only)

Changes in 1.2.2

    * add PHP xDebug 2.0.4
    * update to PHP 5.2.9-2
    * update to MySQL 5.1.33-community
    * update to phpMyAdmin 3.1.3.1
    * update to XCache 2.0.0-dev
    * update to Drupal 6.11 (TWAMPd only)

» REQUIREMENTS

    * Windows XP/2000/2003/Vista
    * 50MB hard disk or removable media space

» Installation

decompress to your hdd then execute apmxe.exe

That's it, your TWAMP should work now! In your browser open URL where you have installed TWAMP, for example:
http://localhost/TWAMP or http://127.0.0.1/TWAMP
phpMyAdmin http://localhost/phpMyAdmin/  (login id: root, password is empty)

TWAMP\htdocs is the DocumentRoot of your local server.

You can change this by edit httpd.conf which in TWAMP\Apache\conf

» Upgrading from TWAMP version 1.2.x

Please take 5 minutes time and read upgrade instructions carefully and completely! This will ensure a proper and easy upgrade.

   1. Stop apmxe.exe then quit this program.
   2. backup all existing TWAMP\htdocs and TWAMP\MySQL\data files!
   3. overwrite all TWAMP files then restor what you backup.
   4. Re-start apmxe.exe

» HELP and Troubleshooting
1. Could not open url http://localhost or http://127.0.0.1/index.php

check if apache is running, before start TWAMP, make sure your system port 80 and 443 is free to use.
or you can change default port number by edit httpd.conf in TWAMP\Apache\conf
Search Listen 80 in httpd.conf
change 80 to other number from 1 to 65535
2. Could not start MySQL server

make sure your system port 3306 is free to use.
or you can change default port number by edit my.ini in TWAMP\MySQL
Search port = 3306 in my.ini
Change 3306 to other number from 1 to 65535
3. How to enbale PHP xCache byte code accelerator

Edit php.ini in TWAMPd\PHP
Search line
;extension = php_xcache.dll
change to
extension = php_xcache.dll
4. How to enbale PHP APC byte code accelerator

Edit php.ini in TWAMPd\PHP
Search line
;extension=php_apc.dll
change to
extension=php_apc.dll
5. How to enbale PHP eAccelerator byte code accelerator

Edit php.ini in TWAMPd\PHP
Search line
;zend_extension_ts = ./ext/eAccelerator_ts.dll
change to
zend_extension_ts = ./ext/eAccelerator_ts.dll
