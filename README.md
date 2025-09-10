# Accepting Paid Request! Discord: Slayer47#7002
# Donation
<a href="https://www.buymeacoffee.com/slayer47" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>

# [PayPal](https://www.paypal.me/ZakriaMansoor)

## Description:
Show the Kill Icon and play the kill sound on killing a player.

## Installation:
- Download and extract it to your server **addons/counterstrikesharp/**
- You would need to install this **[Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3565380064)** addon, if you want to use Valorant/Battlefield killsounds.

## Config:
```json
{
  "ShowKillInfo": true,  // Show Text Kill Info Message in center
  "KillInfoMessage": "\u003Cbr\u003E\u003Cfont class=\u0027fontSize-m\u0027 color=\u0027red\u0027\u003EKilled\u003C/font\u003E \u003Cfont class=\u0027fontSize-m\u0027 color=\u0027lime\u0027\u003E{PlayerName}\u003C/font\u003E \u003Cfont class=\u0027fontSize-m\u0027 color=\u0027gold\u0027\u003E[{WeaponName}]\u003C/font\u003E", // Kill Info Message
  "LoopIfKillIconsEnd": true, // Loop the Killstreak if kills count becomes greater than available KillIcons.
  "ExtendKillStreakIcons": false, // Extend Kill streak icons instead of replacing them after killing.
  "SoundEventPath": "soundevents/slayer-killstreak.vsndevts", // sound event path for the killstreak
  "SoundVolume": 1.0, // Sound Volume for the killstreak
  "KillIcons": {
    "1": // Unique Kill Count Number. (this is for the first kill)
    { 
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/PLL04Q8/kill1.png\" alt=\"kill1\" border=\"0\"></a>", // Icon source html link
      "Sound": "Kill.Sound_01", // Kill Sound Title in sound event file
      "Duration": 3.0 // How long this kill icon should be displayed
    },
    "2": {
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/h1F9FSpk/kill2.png\" alt=\"kill2\" border=\"0\"></a>",
      "Sound": "Kill.Sound_02",
      "Duration": 3.0
    },
    "3": {
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/FLt4JW6h/kill3.png\" alt=\"kill3\" border=\"0\"></a>",
      "Sound": "Kill.Sound_03",
      "Duration": 3.0
    },
    "4": {
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/F4d16TXh/kill4.png\" alt=\"kill4\" border=\"0\"></a>",
      "Sound": "Kill.Sound_04",
      "Duration": 3.0
    },
    "5": {
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/PZfvmKDY/kill5.png\" alt=\"kill5\" border=\"0\"></a>",
      "Sound": "Kill.Sound_05",
      "Duration": 5.5
    },
    "6": {
      "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/BKY6SsdC/kill6.png\" alt=\"kill6\" border=\"0\"></a>",
      "Sound": "Kill.Sound_06",
      "Duration": 5.5
    }
  },
  "HeadshotIcons": {},  // Same format as above (KillIcons). If you have special icons for headshot kills, then add them here. if HeadshotIcons is empty, then it will use KillIcons instead if a headshot kill happens.
  "ConfigVersion": 1
}
```

## To use Battlefield Icons:
```json
{  
    ExtendKillStreakIcons: true,
    "KillIcons": {
      "1": // Unique Kill Count Number. (This is for the first kill)
      { 
        "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/93fMBmcB/kill.png\" alt=\"kill\" border=\"0\"></a>", // Icon source html link
        "Sound": "BF.Kill", // Kill Sound Title in sound event file
        "Duration": 3.0 // How long this kill icon should be displayed
      }
    }
    "HeadshotIcons": {
      "1": // Unique Headshot Kill Count Number. (This is for the first headshot kill)
      { 
        "Icon": "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/wZDrtkxG/headshot.png\" alt=\"headshot\" border=\"0\"></a>", // Icon source html link
        "Sound": "BF.Headshot", // Kill Sound Title in sound event file
        "Duration": 3.0 // How long this kill icon should be displayed
      }
    } 
}
```
