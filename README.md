# GEVOTicket

This program enables the detailed stats functionality on https://gevostats.com

## Why?
  GEVO does not allow other players to view your detailed careers stats. Because of this limitation, this step is required.

## How?  

  Games and programs on Steam all utilize the Steamworks API for integrating with Steam. This program also utilizes the Steamworks API to generate an authentication ticket that can be passed to https://gevostats.com to communicate with the GEVO servers. [^1]  
  After we communicate this ticket to the GEVO servers, we are able to get a short lived JWT that allows us to retrieve detailed career stats.  
  Once the stats are retrieved, the JWT is allowed to expire and the Steam authentication ticket is cancelled.

## Is this secure?

  Under normal circumstances, the authentication ticket from Steam only contains enough information for a dedicated server (or peer in P2P games) to prove the identity of the remote user. [^1]  
  
  These tickets are single-use and are sent to every single multiplayer server (or peer in P2P.) Functionally transmitting them should be *secure enough*.  No one can hijack your Steam account with an authentication token alone.  
  
  The larger security risk actually comes from Bandai in this authentication flow. 
  
  Bandai assumes that as long as the token is valid, then the user is valid and makes no additional efforts to continually verify the user once the JWT is issued.
  One niceity of the JWT is that it is extremely short-lived and expires if there is not a constant heart-beat to the GEVO servers.
  
  We plan on open sourcing as much of our API that is reasonably possible, including the GEVO server authentication flow, but ultimately there will always have to be some level of trust.  
  
  The detailed stats functionality is however entirely optional and the rest of the website will function even if you opt-out of this feature.

## Why can't I just send you my stats and keep the token local?
  This is to prevent people from sending falsified or otherwise harmful stats. Without the token, then there's no way for us to authenticate that you *really are you*
  
  We do plan on releasing an option to locally dump your own stats so that you can feed them into whichever software you would like, **without sending us a ticket.**
 [^1]: https://partner.steamgames.com/doc/features/auth#client_to_client
