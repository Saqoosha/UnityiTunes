//
//  UnityiTunes.m
//  UnityiTunes
//
//  Created by Saqoosha on 2015/02/17.
//  Copyright (c) 2015 Saqoosha. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "iTunes.h"

typedef void (*callbackFunc)(const char *);


@interface iTunesHelper : NSObject {
  iTunesApplication *iTunes_;
  callbackFunc statusCallback_;
  char artistBuffer[1024];
  char titleBuffer[1024];
}
@property callbackFunc statusCallback;
@end

@implementation iTunesHelper

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
  }
  return self;
}

- (void)dealloc
{
  [[NSDistributedNotificationCenter defaultCenter] removeObserver:self];
  [iTunes_ release];
  [super dealloc];
}

- (void)updateTrackInfoFromITunes:(NSNotification *)notification
{
  NSString *state = notification.userInfo[@"Player State"];
  if (statusCallback_ != NULL)
  {
    statusCallback_([state UTF8String]);
  }
}

- (double)playerPosition
{
  return iTunes_.playerPosition;
}

- (const char *)artist
{
  strncpy(artistBuffer, [iTunes_.currentTrack.artist UTF8String], 1023);
  artistBuffer[1023] = '\0';
  return artistBuffer;
}

- (const char *)title
{
  strncpy(titleBuffer, [iTunes_.currentTrack.name UTF8String], 1023);
  titleBuffer[1023] = '\0';
  return titleBuffer;
}

@end


static iTunesHelper *helper = nil;

void _Init(callbackFunc callback)
{
  if (!helper) helper = [[iTunesHelper alloc] init];
  helper.statusCallback = callback;
}

void _Cleanup()
{
  [helper dealloc];
  helper = nil;
}

double _GetPlayPosition()
{
  return helper.playerPosition;
}

const char *_GetArtist()
{
  return helper.artist;
}

const char *_GetTitle()
{
  return helper.title;
}

