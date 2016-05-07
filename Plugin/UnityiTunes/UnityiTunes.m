//
//  UnityiTunes.m
//  UnityiTunes
//
//  Created by Saqoosha on 2015/02/17.
//  Copyright (c) 2015 Saqoosha. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "iTunes.h"

#define BUFLEN (1024)


typedef void (*callbackFunc)(const char *);


@interface iTunesHelper : NSObject
{
  iTunesApplication *iTunes_;
  callbackFunc statusCallback_;
  char *artist_;
  char *album_;
  char *title_;
  float duration_;
}

@property(readonly) iTunesApplication *iTunes;
@property(readonly) char *artist;
@property(readonly) char *album;
@property(readonly) char *title;
@property(readonly) float duration;
@property callbackFunc statusCallback;

@end



@implementation iTunesHelper

@synthesize iTunes = iTunes_;
@synthesize artist = artist_;
@synthesize album = album_;
@synthesize title = title_;
@synthesize duration = duration_;
@synthesize statusCallback = statusCallback_;


- (id) init
{
  self = [super init];
  if (self)
  {
    iTunes_ = [[SBApplication applicationWithBundleIdentifier:@"com.apple.iTunes"] retain];
    [[NSDistributedNotificationCenter defaultCenter] addObserver:self
                                                        selector:@selector(updateTrackInfoFromITunes:)
                                                            name:@"com.apple.iTunes.playerInfo"
                                                          object:nil];
    artist_ = calloc(BUFLEN, 1);
    album_ = calloc(BUFLEN, 1);
    title_ = calloc(BUFLEN, 1);
  }
  return self;
}


- (void)dealloc
{
  free(artist_);
  free(album_);
  free(title_);
  
  [[NSDistributedNotificationCenter defaultCenter] removeObserver:self];
  [iTunes_ release];
  
  [super dealloc];
}


- (void)updateTrackInfoFromITunes:(NSNotification *)notification
{
  NSLog(@"%@", notification.userInfo);
  

  memset(artist_, 0, BUFLEN);
  if (notification.userInfo[@"Artist"]) {
    strncpy(artist_, [notification.userInfo[@"Artist"] UTF8String], BUFLEN - 1);
  }

  memset(album_, 0, BUFLEN);
  if (notification.userInfo[@"Album"]) {
    strncpy(album_, [notification.userInfo[@"Album"] UTF8String], BUFLEN - 1);
  }

  memset(title_, 0, BUFLEN);
  if (notification.userInfo[@"Name"]) {
    strncpy(title_, [notification.userInfo[@"Name"] UTF8String], BUFLEN - 1);
  }
  
  duration_ = [notification.userInfo[@"Total Time"] floatValue] / 1000.0; // ms to secs
  
  NSString *state = notification.userInfo[@"Player State"];
  if (statusCallback_ != NULL)
  {
    statusCallback_([state UTF8String]);
  }
}


@end



#pragma mark - Plugin Interface


static iTunesHelper *helper = nil;


void _Init(callbackFunc callback)
{
  if (!helper) helper = [[iTunesHelper alloc] init];
  helper.statusCallback = callback;
}


void Cleanup()
{
  [helper dealloc];
  helper = nil;
}


void PlayPause()
{
  [helper.iTunes playpause];
}


void Rewind()
{
  [helper.iTunes rewind];
}


void Stop()
{
  [helper.iTunes stop];
}


int _GetStatus()
{
  return helper.iTunes.playerState;
}


double _GetPlayPosition()
{
  return helper.iTunes.playerPosition;
}

void _SetPlayPosition(double position)
{
  helper.iTunes.playerPosition = position;
}


const char *_GetArtist()
{
  return helper.artist;
}


const char *_GetAlbum()
{
  return helper.album;
}


const char *_GetTitle()
{
  return helper.title;
}


double _GetDuration()
{
  return helper.duration;
}

