�
	    0  t�         p 	      !        t  ��          //                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 (     �   @���   �     ��        �  @ ) �   ��   ��PRIMARY�timestamp�uid�ssid�                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     �                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 MyISAM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   �                                           6Drupal’s session handlers read and write into the...                                                                                                                                                             s �  �0         P   �  s )                                          uid  sid  ssid  	hostname  
timestamp 	 cache 
 session 

   @   !B K�   @   !D J��      !I 	F�
      !4 
 �     !h 	 �     !x 
 � �  �?� �uid�sid�ssid�hostname�timestamp�cache�session� The users.uid corresponding to a session, or 0 for anonymous user.A session ID. The value is generated by Drupal’s session handlers.Secure session ID. The value is generated by Drupal’s session handlers.The IP address that last used this session ID (sid).The Unix timestamp when this session last requested a page. Old records are purged by PHP automatically.The time of this user’s last post. This is used when the site has specified a minimum_cache_lifetime. See cache_get().The serialized contents of $_SESSION, an array of name/value pairs that persists across page requests by this session ID. Drupal loads $_SESSION from here at the start of each request and saves it at the end.